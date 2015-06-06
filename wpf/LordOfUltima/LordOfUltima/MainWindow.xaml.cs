using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LordOfUltima.Events;
using LordOfUltima.MGameboard;
using LordOfUltima.RessourcesProduction;
using LordOfUltima.Web;
using LordOfUltima.XML;

namespace LordOfUltima
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        
        public const int chatbox_max_items = 30;
        private RessourcesManager _ressourcesManager;

        // REFACTOR START
        private IdleUiThread _idleUiThread;
        private UIImagesInit _imagesInit;
        private User.User _user;
        private readonly Gameboard _gameboard;
        private GameboardInit _gameboardInit;
        private LevelIndicatorVisibility _levelIndicatorVisibility;
        private BuildingMenuVisibility _buildingMenuVisibility;
        private BuildingDetailsVisibility _buildingDetailsVisibility;
        private ChatEvents _chatEvents;
        // REFACTOR END

        public static MainWindow m_ins;
        public MainWindow()
        {
            m_ins = this;
            InitializeComponent();

            //REFACTOR START

            // Set the user instance
            _user = User.User.getInstance();
            label_player_name.Content = _user.Name;

            // Dispatcher pour programme Idle
            _idleUiThread = IdleUiThread.Instance;
            ComponentDispatcher.ThreadIdle += _idleUiThread.IdleThreadWork;

            // Insert images in UI
            _imagesInit = UIImagesInit.Instance;
            _imagesInit.InitImages();

            // Set the gameboard Instance
            _gameboard = Gameboard.getInstance();

            // Insertion des elements dans la carte
            _gameboardInit = GameboardInit.Instance;
            _gameboardInit.InsertMap();

            // Hide level indicators
            _levelIndicatorVisibility = LevelIndicatorVisibility.Instance;
            _levelIndicatorVisibility.hideLevelIndicator();

            // Hide building menu
            _buildingMenuVisibility = BuildingMenuVisibility.Instance;
            _buildingMenuVisibility.hideBuildingMenu();

            // Hide Building Details menu
            _buildingDetailsVisibility = BuildingDetailsVisibility.Instance;
            _buildingDetailsVisibility.hideBuildingDetails();
            
            // load game
            if (!SaveGame.Instance.Load())
            {
                // If no game is found, load a new one
                _gameboard.initialiseNewGame();
            }

            // Start Ressource management
            _ressourcesManager = RessourcesManager.Instance;
            _ressourcesManager.StartRessourcesManager();

            _chatEvents = ChatEvents.Instance;

            //REFACTOR END

            // Start Chat thread
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += updateChat;
            bw.RunWorkerAsync();
        }

        #region Map Mouvement
        const double ScaleRate = 1.05;
        const double ScaleMin = 1.0;
        const double ScaleMax = 2.5;
        private Point m_p = new Point(0, 0);
        private double m_scale = 1;
        private bool m_isset = false;
        /*
         * Gestion du scroll Wheel dans le canvas
        */
        private void canvas1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0 && !m_isset)
            {
                m_isset = true;
                m_p = e.MouseDevice.GetPosition(canvas1);
            }

            Matrix m = canvas1.RenderTransform.Value;
            if (e.Delta > 0)
            {
                if(m_scale * ScaleRate > ScaleMax)
                {
                    return;
                }
                m.ScaleAtPrepend(ScaleRate, ScaleRate, m_p.X, m_p.Y);
                m_scale *= ScaleRate;
            }
            else
            {
                if (m_scale / ScaleRate < ScaleMin)
                {
                    m_isset = false;
                    return;
                }
                m.ScaleAtPrepend(1 / ScaleRate, 1 / ScaleRate, m_p.X, m_p.Y);
                m_scale /= ScaleRate;
            }
            canvas1.RenderTransform = new MatrixTransform(m);
        }

        /*
         * Gestion du drag
        */
        private bool m_isMouseDown = false;
        private static bool m_isMouseMove = false;
        private Point m_start_mouse_pos;
        private Point m_old_pos = new Point(0, 0);
        private void canvas1_leftMouseDown(object sender, RoutedEventArgs e)
        {
            m_isMouseDown = true;
            // Reinitialiser le mouve a false au click down
            m_isMouseMove = false;
            m_totalMove = 0.0;

            m_start_mouse_pos = Mouse.GetPosition(canvas_mouse_pos);
            canvas1.CaptureMouse();
        }

        private void canvas1_leftMouseUp(object sender, RoutedEventArgs e)
        {
            m_isMouseDown = false;
            canvas1.ReleaseMouseCapture();
            m_old_pos.X = Canvas.GetLeft(canvas1);
            m_old_pos.Y = Canvas.GetTop(canvas1);
        }

        public static bool getIsMouseMove()
        {
            return m_isMouseMove;
        }
        /*
         * Gestion du deplacement de la souris sur la carte
        */
        private double m_totalMove = 0;
        private void canvas1_mouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!m_isMouseDown) return;
            
            // Si on se deplace de plus d'un certain nombre de pixel, on ignore le click up pour la selection d'un batiment
            if(m_totalMove > 5)
            {
                m_isMouseMove = true;
            }

            Point currentMousePos = Mouse.GetPosition(canvas_mouse_pos);

            double max_deplacement = 100 * Math.Pow(m_scale,3);
            double dx = m_old_pos.X + (currentMousePos.X - m_start_mouse_pos.X);
            double dy = m_old_pos.Y + (currentMousePos.Y - m_start_mouse_pos.Y);

            // Calcul du mouse move totale
            m_totalMove += dx + dy;

            if (Math.Abs(dx) < max_deplacement )
                Canvas.SetLeft(canvas1, dx);

            if (Math.Abs(dy) < max_deplacement)
                Canvas.SetTop(canvas1, dy);
        }
        #endregion

        #region Chat
        /*
         * Gestion entrer clavier pour le chat text input
        */
        private void chat_text_KeyDown(object sender, KeyEventArgs e)
        {
            //_chatEvents.ChatKeyDown(sender,e);

            if(e.Key == Key.Enter)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(m_chat_ins.insertNewChatLine));
                thread.Start(chat_text.Text);
                chat_text.Text = "";
            }
        }

        private bool m_stop_chat_update = false;
        private Chat m_chat_ins = Chat.getInstance();
        private List<string> m_new_chat_lines = new List<string>();
        private Object m_lock_chat = new Object();

        private void updateChat(object sender, DoWorkEventArgs e)
        {
            while (!m_stop_chat_update)
            {
                // update each 1 seconds
                System.Threading.Thread.Sleep(1000);

                if(!m_chat_ins.is_init)
                {
                    m_chat_ins.initChat();
                }
                else
                {
                    lock (m_lock_chat)
                    {
                        m_new_chat_lines = m_chat_ins.getLastChatString();
                    }
                }
            }
        }

        private List<string> m_chat_text = new List<string>();
        public void ui_thread_updateChat()
        {
            if (m_new_chat_lines != null && m_new_chat_lines.Count() != 0)
            {
                lock (m_lock_chat)
                {
                    m_new_chat_lines.ForEach(delegate(String chatText)
                    {
                        m_chat_text.Add(chatText);
                    });
                    m_new_chat_lines.Clear();
                }
                textbox_scroll_viewer.ScrollToBottom();

                // limit the number in the chat box
                if (m_chat_text.Count > chatbox_max_items)
                {
                    m_chat_text.RemoveAt(0);
                }
                string text = String.Join(Environment.NewLine, m_chat_text);
                chat_textbox.Text = text;
            }
        }
        #endregion


        #region Menu element click
        /*
         * Trigger building level indicator on map
        */
        private void trigger_level_Click(object sender, RoutedEventArgs e)
        {
            _levelIndicatorVisibility.handleLevelIndicatorVisibility();
        }

        /*
         * Reset the current map
        */
        private void reset_map_Click(object sender, RoutedEventArgs e)
        {
            // Show messagebox for confirmation
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to reset map?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _gameboard.resetMap();
                // Init a new game
                _gameboard.initialiseNewGame();

                _buildingMenuVisibility.hideBuildingMenu();
                _buildingDetailsVisibility.hideBuildingDetails();
            }
        }

        /*
         * Logout of the game
        */
        private void logout_Click(object sender, RoutedEventArgs e)
        {
            foreach (Element element in _gameboard.getMap())
            {
                canvas1.Children.Remove(element.getElement());
                canvas1.Children.Remove(element.getLevelElement());
                canvas1.Children.Remove(element.getLevelLabel());
                canvas1.Children.Remove(element.getSelectElement());
            }
            _gameboard.resetMap();
            LoginWindow window = new LoginWindow();
            window.Show();
            Close();
        }

        /*
         * Save Game
        */
        private void game_save_Click(object sender, RoutedEventArgs e)
        {
            SaveGame.Instance.Save();
        }

        /*
         * Load the last saved game
        */
        private void game_load_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to load the last save?", "Load Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _gameboard.resetMap();
                SaveGame.Instance.Load();
            }
        }
        #endregion

        #region SideMenu building Click
        private BuildEvent m_buildEvent = BuildEvent.getInstance();
        private void building_woodcutter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new WoodcutterElementType());
        }

        private void building_sawmill_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new SawmillElementType());
        }

        private void building_quarry_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new QuarryElementType());
        }

        private void building_stonemason_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new StoneMasonElementType());
        }

        private void building_ironmine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new IronMineElementType());
        }

        private void building_foundry_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new FoundryElementType());
        }

        private void building_farm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new FarmElementType());
        }

        private void building_mill_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BuildElement(new MillElementType());
        }

        private void BuildElement(IElementType element)
        {
            m_buildEvent.setTypeToBuild(element);
            m_buildEvent.buildElement();
            m_buildEvent.setElementToBuild(null);
            BuildingMenuVisibility.Instance.hideBuildingMenu();
        }
        #endregion

        // update instance on windows closing
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            m_ins = null;
        }
    }
}
