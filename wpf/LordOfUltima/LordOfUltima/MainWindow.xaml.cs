using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using LordOfUltima.Events;
using LordOfUltima.MGameboard;
using LordOfUltima.RessourcesProduction;
using LordOfUltima.XML;

namespace LordOfUltima
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Gameboard _gameboard;
        private readonly LevelIndicatorVisibility _levelIndicatorVisibility;
        private readonly BuildingMenuVisibility _buildingMenuVisibility;
        private readonly BuildingDetailsVisibility _buildingDetailsVisibility;
        private readonly ChatEvents _chatEvents;

        public static MainWindow MIns;
        public MainWindow()
        {
            MIns = this;
            InitializeComponent();

            // Set the gameboard Instance
            _gameboard = Gameboard.Instance;
            ResetMap.Instance.VerifyMap();

            // Set the user instance
            label_player_name.Content = User.User.Instance.Name;

            // Dispatcher pour programme Idle
            ComponentDispatcher.ThreadIdle += IdleUiThread.Instance.IdleThreadWork;

            // Insert images in UI
            UIImagesInit.Instance.InitImages();

            // Insertion des elements dans la carte
            GameboardInit.Instance.InsertMap();

            // Hide level indicators
            _levelIndicatorVisibility = LevelIndicatorVisibility.Instance;
            _levelIndicatorVisibility.HideLevelIndicator();

            // Hide building menu
            _buildingMenuVisibility = BuildingMenuVisibility.Instance;
            _buildingMenuVisibility.HideBuildingMenu();

            // Hide Building Details menu
            _buildingDetailsVisibility = BuildingDetailsVisibility.Instance;
            _buildingDetailsVisibility.HideBuildingDetails();
            
            // load game
            if (!SaveGame.Instance.Load())
            {
                // If no game is found, load a new one
                ResetMap.Instance.InitialiseNewGame();
            }

            // Start Ressource management
            RessourcesManager.Instance.StartRessourcesManager();

            _chatEvents = ChatEvents.Instance;

            // Start Chat thread
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += _chatEvents.UpdateChat;
            bw.RunWorkerAsync();
        }

        #region Map Mouvement
        const double ScaleRate = 1.05;
        const double ScaleMin = 1.0;
        const double ScaleMax = 2.5;
        private Point _point = new Point(0, 0);
        private double _scale = 1;
        private bool _isset;
        /*
         * Gestion du scroll Wheel dans le canvas
        */
        private void canvas1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0 && !_isset)
            {
                _isset = true;
                _point = e.MouseDevice.GetPosition(canvas1);
            }

            Matrix m = canvas1.RenderTransform.Value;
            if (e.Delta > 0)
            {
                if(_scale * ScaleRate > ScaleMax)
                {
                    return;
                }
                m.ScaleAtPrepend(ScaleRate, ScaleRate, _point.X, _point.Y);
                _scale *= ScaleRate;
            }
            else
            {
                if (_scale / ScaleRate < ScaleMin)
                {
                    _isset = false;
                    return;
                }
                m.ScaleAtPrepend(1 / ScaleRate, 1 / ScaleRate, _point.X, _point.Y);
                _scale /= ScaleRate;
            }
            canvas1.RenderTransform = new MatrixTransform(m);
        }

        /*
         * Gestion du drag
        */
        private bool _isMouseDown;
        private static bool _isMouseMove;
        private Point _startMousePos;
        private Point _oldPos = new Point(0, 0);
        private void canvas1_leftMouseDown(object sender, RoutedEventArgs e)
        {
            _isMouseDown = true;
            // Reinitialiser le mouve a false au click down
            _isMouseMove = false;
            _totalMove = 0.0;

            _startMousePos = Mouse.GetPosition(canvas_mouse_pos);
            canvas1.CaptureMouse();
        }

        private void canvas1_leftMouseUp(object sender, RoutedEventArgs e)
        {
            _isMouseDown = false;
            canvas1.ReleaseMouseCapture();
            _oldPos.X = Canvas.GetLeft(canvas1);
            _oldPos.Y = Canvas.GetTop(canvas1);
        }

        public static bool GetIsMouseMove()
        {
            return _isMouseMove;
        }
        /*
         * Gestion du deplacement de la souris sur la carte
        */
        private double _totalMove;
        private void canvas1_mouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMouseDown) return;
            
            // Si on se deplace de plus d'un certain nombre de pixel, on ignore le click up pour la selection d'un batiment
            if(_totalMove > 5)
            {
                _isMouseMove = true;
            }

            Point currentMousePos = Mouse.GetPosition(canvas_mouse_pos);

            double maxDeplacement = 100 * Math.Pow(_scale,3);
            double dx = _oldPos.X + (currentMousePos.X - _startMousePos.X);
            double dy = _oldPos.Y + (currentMousePos.Y - _startMousePos.Y);

            // Calcul du mouse move totale
            _totalMove += dx + dy;

            if (Math.Abs(dx) < maxDeplacement )
                Canvas.SetLeft(canvas1, dx);

            if (Math.Abs(dy) < maxDeplacement)
                Canvas.SetTop(canvas1, dy);
        }
        #endregion

        /*
         * Gestion entrer clavier pour le chat text input
        */
        private void chat_text_KeyDown(object sender, KeyEventArgs e)
        {
            _chatEvents.ChatKeyDown(sender,e);

        }

        #region Menu element click
        /*
         * Trigger building level indicator on map
        */
        private void trigger_level_Click(object sender, RoutedEventArgs e)
        {
            _levelIndicatorVisibility.HandleLevelIndicatorVisibility();
        }

        /*
         * Reset the current map
        */
        private void reset_map_Click(object sender, RoutedEventArgs e)
        {
            // Show messagebox for confirmation
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to reset map?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ResetMapElements.Instance.ResetMap();
                // Init a new game
                ResetMap.Instance.InitialiseNewGame();

                _buildingMenuVisibility.HideBuildingMenu();
                _buildingDetailsVisibility.HideBuildingDetails();
            }
        }

        /*
         * Logout of the game
        */
        private void logout_Click(object sender, RoutedEventArgs e)
        {
            foreach (Element element in _gameboard.GetMap())
            {
                canvas1.Children.Remove(element.getElement());
                canvas1.Children.Remove(element.getLevelElement());
                canvas1.Children.Remove(element.getLevelLabel());
                canvas1.Children.Remove(element.getSelectElement());
            }
            ResetMapElements.Instance.ResetMap();
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
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to load the last save?", "Load Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ResetMapElements.Instance.ResetMap();
                SaveGame.Instance.Load();
            }
        }
        #endregion

        #region SideMenu building Click
        private readonly BuildEvent _buildEvent = BuildEvent.Instance;
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
            _buildEvent.SetTypeToBuild(element);
            _buildEvent.BuildElement();
            _buildEvent.SetElementToBuild(null);
            BuildingMenuVisibility.Instance.HideBuildingMenu();
        }
        #endregion

        // update instance on windows closing
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MIns = null;
        }
    }
}
