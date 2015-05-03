using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LordOfUltima
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Gameboard m_gameboard;

        public MainWindow()
        {
            InitializeComponent();

            // Couleur du menu
            top_menu.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xE9, 0xD3, 0xAE));
            grid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xE9, 0xD3, 0xAE));

            // Background pour canvas
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@"Media/main.png", UriKind.Relative));
            canvas1.Background = imageBrush;


            LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
            myLinearGradientBrush.StartPoint = new Point(0, 0);
            myLinearGradientBrush.EndPoint = new Point(0, 1);

            myLinearGradientBrush.GradientStops.Add( new GradientStop(Colors.White, -0.03));
            myLinearGradientBrush.GradientStops.Add( new GradientStop(Colors.Transparent, 0.10));
            myLinearGradientBrush.GradientStops.Add( new GradientStop(Colors.Transparent, 0.90));
            myLinearGradientBrush.GradientStops.Add( new GradientStop(Colors.White, 1.03));

            canvas_fog.Fill = myLinearGradientBrush;


            // Backgrouound pour Scroll View
            ImageBrush imageScroll = new ImageBrush();
            imageScroll.ImageSource = new BitmapImage(new Uri(@"Media/menu.png", UriKind.Relative));
            scrollview.Background = imageScroll;

            // Image for wood 
            ImageBrush imageRWood = new ImageBrush();
            imageRWood.ImageSource = new BitmapImage(new Uri(@"Media/ressource/icon/Lou_resource_wood.png", UriKind.Relative));
            ress_wood.Background = imageRWood;

            // Image for stone
            ImageBrush imageRStone = new ImageBrush();
            imageRStone.ImageSource = new BitmapImage(new Uri(@"Media/ressource/icon/Lou_resource_stone.png", UriKind.Relative));
            ress_stone.Background = imageRStone;

            ImageBrush ressMenu = new ImageBrush();
            ressMenu.ImageSource = new BitmapImage(new Uri(@"Media/ress_menu.png", UriKind.Relative));
            ress_menu.Background = ressMenu;

            // Set the gameboard Instance
            m_gameboard = Gameboard.getInstance();
            // Insertion des elements dans la carte
            insertMap();
            
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            double mouseX = Mouse.GetPosition(grid).X;
            double mouseY = Mouse.GetPosition(grid).Y;
        }

        /*
         * Insertion du tableau d'Elements dans le canvas (a l'initialisation de la fenetre)
        */
        private void insertMap()
        {
            foreach(Element element in m_gameboard.getMap())
            {
                canvas1.Children.Add(element.getElement());
            }
        }

        private void insertMenu()
        {

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
        private Point m_start_mouse_pos;
        private Point m_old_pos = new Point(0, 0);
        private void canvas1_leftMouseDown(object sender, RoutedEventArgs e)
        {
            m_isMouseDown = true;
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

        private void canvas1_mouseMove(object sender, MouseEventArgs e)
        {
            if (!m_isMouseDown) return;

            Point currentMousePos = Mouse.GetPosition(canvas_mouse_pos);

            // center the rect on the mouse
            double max_deplacement = 100 * Math.Pow(m_scale,3);
            double dx = m_old_pos.X + (currentMousePos.X - m_start_mouse_pos.X);
            double dy = m_old_pos.Y + (currentMousePos.Y - m_start_mouse_pos.Y);

            if (Math.Abs(dx) < max_deplacement )
                Canvas.SetLeft(canvas1, dx);

            if (Math.Abs(dy) < max_deplacement)
                Canvas.SetTop(canvas1, dy);

           // m_start_mouse_pos = currentMousePos;
        }

    }
}
