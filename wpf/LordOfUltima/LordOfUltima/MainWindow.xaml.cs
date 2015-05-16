using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using LordOfUltima.Web;

namespace LordOfUltima
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Gameboard m_gameboard;
        private Stopwatch m_watch;
        private User.User m_user;
        public const int chatbox_max_items = 30;

        public static MainWindow m_ins = null;
        public MainWindow()
        {
            m_ins = this;
            InitializeComponent();

            initImages();

            // Set the user instance
            m_user = User.User.getInstance();
            label_player_name.Content = m_user.Name;

            // Set the gameboard Instance
            m_gameboard = Gameboard.getInstance();
            // Insertion des elements dans la carte
            insertMap();
            // Inserer les elements dans le menu
            insertMenu();
            // Cacher les niveaux a la base
            m_gameboard.hideLevelIndicator();

            // Init a new game
            m_gameboard.initialiseNewGame();

            // hide building menu
            setVisibleBuildingMenu(false);

            // Start Stop watch
            m_watch = new Stopwatch();
            m_watch.Start();

            // Dispatcher pour programme Idle
            ComponentDispatcher.ThreadIdle += new System.EventHandler(ComponentDispatcher_ThreadIdle);

            // Start Chat thread
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += updateChat;
            bw.RunWorkerAsync();
        }

        // Idle loop
        void ComponentDispatcher_ThreadIdle(object sender, EventArgs e)
        {
            //do your idle stuff here
            System.Threading.Thread.Sleep(10);
            // Show stopwatch in menu :D
            TimeSpan ts = m_watch.Elapsed;
            stop_watch.Content = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            // Update the chat 
            ui_thread_updateChat();
        }

        /*
         * Insertion du tableau d'Elements dans le canvas (a l'initialisation de la fenetre)
        */
        private void insertMap()
        {
            foreach(Element element in m_gameboard.getMap())
            {
                // Add building to canvas
                canvas1.Children.Add(element.getElement());
                // Add building level to canvas
                canvas1.Children.Add(element.getLevelElement());
                // Add level label to canvas
                canvas1.Children.Add(element.getLevelLabel());
                // Add select rect to canvas
                canvas1.Children.Add(element.getSelectElement());
                
            }
        }

        private void insertMenu()
        {
            // Image for Woodcutter
            ImageBrush imageWoodcutter = new ImageBrush();
            imageWoodcutter.ImageSource = new BitmapImage(new Uri(@"Media/menu/menu_woodcutter.png", UriKind.Relative));
            building_woodcutter.Fill = imageWoodcutter;

            // Image for Sawmill
            ImageBrush imageSawmill = new ImageBrush();
            imageSawmill.ImageSource = new BitmapImage(new Uri(@"Media/menu/menu_sawmill.png", UriKind.Relative));
            building_sawmill.Fill = imageSawmill;

            // Image for Quarry
            ImageBrush imageQuarry = new ImageBrush();
            imageQuarry.ImageSource = new BitmapImage(new Uri(@"Media/menu/menu_quarry.png", UriKind.Relative));
            building_quarry.Fill = imageQuarry;

            // Image for StoneMason
            ImageBrush imageStonemason = new ImageBrush();
            imageStonemason.ImageSource = new BitmapImage(new Uri(@"Media/menu/menu_stonemason.png", UriKind.Relative));
            building_stonemason.Fill = imageStonemason;

            // Image for Iron mine
            ImageBrush imageIronmine = new ImageBrush();
            imageIronmine.ImageSource = new BitmapImage(new Uri(@"Media/menu/menu_ironmine.png", UriKind.Relative));
            building_ironmine.Fill = imageIronmine;

            // Image for Foundry
            ImageBrush imageFoundry = new ImageBrush();
            imageFoundry.ImageSource = new BitmapImage(new Uri(@"Media/menu/menu_foundry.png", UriKind.Relative));
            building_foundry.Fill = imageFoundry;
        }

        private void initImages()
        {

            // Couleur du menu
            ImageBrush imageMenuRepeat = new ImageBrush();
            imageMenuRepeat.ImageSource = new BitmapImage(new Uri(@"Media/menu_repeat.png", UriKind.Relative));
            menu1.Background = imageMenuRepeat;
            top_menu.Background = imageMenuRepeat;

            grid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xE9, 0xD3, 0xAE));

            // Background pour canvas
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@"Media/main.png", UriKind.Relative));
            canvas1.Background = imageBrush;

            // Background pour le degrade (Fog)
            LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
            myLinearGradientBrush.StartPoint = new Point(0, 0);
            myLinearGradientBrush.EndPoint = new Point(0, 1);
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.White, -0.03));
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.10));
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0.90));
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.White, 1.03));
            canvas_fog.Fill = myLinearGradientBrush;


            // Background pour Scroll View
            ImageBrush imageScroll = new ImageBrush();
            imageScroll.ImageSource = new BitmapImage(new Uri(@"Media/menu.png", UriKind.Relative));
            scrollview.Background = imageScroll;

            // Image pour le menu des ressources
            ImageBrush ressMenu = new ImageBrush();
            ressMenu.ImageSource = new BitmapImage(new Uri(@"Media/ress_menu.png", UriKind.Relative));
            ress_menu.Background = ressMenu;

            // Image for wood 
            ImageBrush imageRWood = new ImageBrush();
            imageRWood.ImageSource = new BitmapImage(new Uri(@"Media/ressource/icon/Lou_resource_wood.png", UriKind.Relative));
            ress_wood.Background = imageRWood;

            // Image for stone
            ImageBrush imageRStone = new ImageBrush();
            imageRStone.ImageSource = new BitmapImage(new Uri(@"Media/ressource/icon/Lou_resource_stone.png", UriKind.Relative));
            ress_stone.Background = imageRStone;

            // Image for grain
            ImageBrush imageRGrain = new ImageBrush();
            imageRGrain.ImageSource = new BitmapImage(new Uri(@"Media/ressource/icon/Lou_resource_grain.png", UriKind.Relative));
            ress_grain.Background = imageRGrain;

            // Image for iron
            ImageBrush imageRIron = new ImageBrush();
            imageRIron.ImageSource = new BitmapImage(new Uri(@"Media/ressource/icon/Lou_resource_iron.png", UriKind.Relative));
            ress_iron.Background = imageRIron;

            // Image for gold
            ImageBrush imageRGold = new ImageBrush();
            imageRGold.ImageSource = new BitmapImage(new Uri(@"Media/ressource/icon/Lou_resource_gold.png", UriKind.Relative));
            ress_gold.Background = imageRGold;

            // Image for researh
            ImageBrush imageRResearch = new ImageBrush();
            imageRResearch.ImageSource = new BitmapImage(new Uri(@"Media/ressource/icon/research.png", UriKind.Relative));
            ress_research.Background = imageRResearch;
        }


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

        /*
         * Trigger building level indicator on map
        */
        private void trigger_level_Click(object sender, RoutedEventArgs e)
        {
            if(trigger_level.IsChecked)
            {
                m_gameboard.showLevelIndicator();
            }
            else
            {
                m_gameboard.hideLevelIndicator();
            }
        }

        public static bool getIsMouseMove()
        {
            return m_isMouseMove;
        }

        /*
         * Gestion entrer clavier pour le chat text input
        */
        private void chat_text_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
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
        private void ui_thread_updateChat()
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

        private void reset_map_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to reset map?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                m_gameboard.resetMap();
                // Init a new game
                m_gameboard.initialiseNewGame();
            }

        }

        private void logout_Click(object sender, RoutedEventArgs e)
        {
            foreach(Element element in m_gameboard.getMap())
            {
                canvas1.Children.Remove(element.getElement());
                canvas1.Children.Remove(element.getLevelElement());
                canvas1.Children.Remove(element.getLevelLabel());
                canvas1.Children.Remove(element.getSelectElement());
            }
            m_gameboard.resetMap();
            LoginWindow window = new LoginWindow();
            window.Show();
            this.Close();
        }

        public void setVisibleBuildingMenu(bool isVisible)
        {
            if (isVisible)
            {
                building_menu.Visibility = Visibility.Visible;
            }
            else
            {
                building_menu.Visibility = Visibility.Hidden;               
            }


        }

        private BuildEvent m_buildEvent = BuildEvent.getInstance();
        private void building_woodcutter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_buildEvent.setTypeToBuild(new WoodcutterElementType());
            m_buildEvent.buildElement();
            m_buildEvent.setElementToBuild(null);
            setVisibleBuildingMenu(false);
        }

        private void building_sawmill_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_buildEvent.setTypeToBuild(new SawmillElementType());
            m_buildEvent.buildElement();
            m_buildEvent.setElementToBuild(null);
            setVisibleBuildingMenu(false);
        }
    }
}
