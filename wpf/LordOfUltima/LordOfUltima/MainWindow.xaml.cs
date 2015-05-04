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

            initImages();

            // Set the gameboard Instance
            m_gameboard = Gameboard.getInstance();
            // Insertion des elements dans la carte
            insertMap();
            // Inserer les elements dans le menu
            insertMenu();
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
                // Add building to canvas
                canvas1.Children.Add(element.getElement());
                // Add building level to canvas
                canvas1.Children.Add(element.getLevelElement());
                // Add level label to canvas
                //canvas1.Children.Add(element.getLevelLabel());
                canvas1.Children.Add(element.getLevelLabel());
            }
        }

        private void insertMenu()
        {
            // TODO : Insertion des elements graphique dans le menu
        }

        private void initImages()
        {

            // Couleur du menu
            ImageBrush imageMenuRepeat = new ImageBrush();
            imageMenuRepeat.ImageSource = new BitmapImage(new Uri(@"Media/menu_repeat.png", UriKind.Relative));
            /*imageMenuRepeat.TileMode = TileMode.FlipX;
            imageMenuRepeat.Stretch = Stretch.Uniform;
            imageMenuRepeat.Viewport = new Rect(0, 0, 92, 61);
            imageMenuRepeat.ViewportUnits = BrushMappingMode.Absolute;*/
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
