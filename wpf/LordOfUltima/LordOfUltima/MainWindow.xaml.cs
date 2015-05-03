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
            grid.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xE9, 0xD3, 0xAE)); ;

            // Background pour canvas
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@"Media/main.png", UriKind.Relative));
            canvas1.Background = imageBrush;

            // Backgrouound pour Scroll View
            ImageBrush imageScroll = new ImageBrush();
            imageScroll.ImageSource = new BitmapImage(new Uri(@"Media/menu.png", UriKind.Relative));
            scrollview.Background = imageScroll;

            // Image for wood 
            ImageBrush imageRWood = new ImageBrush();
            imageRWood.ImageSource = new BitmapImage(new Uri(@"Media/ressource/icon/Lou_resource_wood.png", UriKind.Relative));
            ress_wood.Background = imageRWood;

            // Set the gameboard Instance
            m_gameboard = Gameboard.getInstance();
            // Insertion des elements dans la carte
            insertMap();
            
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            double mouseX = Mouse.GetPosition(grid).X;
            double mouseY = Mouse.GetPosition(grid).Y;

            // Obtention de la position dans le tableau (prend en compte le zoom)
            frameX.Content = "Frame X : " + m_gameboard.getXFrame(mouseX/st.ScaleX);
            frameY.Content = "Frame Y : " + m_gameboard.getYFrame(mouseY/st.ScaleY);
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

        /*
         * Gestion du scroll Wheel dans le canvas
        */
        const double ScaleRate = 1.1;
        const double ScaleMin = 1.0;
        const double ScaleMax = 1.7;
        private void canvas1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            /*Point p = e.MouseDevice.GetPosition(canvas1);

            Matrix m = canvas1.RenderTransform.Value;
            if (e.Delta > 0)
                m.ScaleAtPrepend(1.1, 1.1, p.X, p.Y);
            else
                m.ScaleAtPrepend(1 / 1.1, 1 / 1.1, p.X, p.Y);

            canvas1.RenderTransform = new MatrixTransform(m);*/
            

           if(e.Delta>0)
           {
               if(st.ScaleX * ScaleRate >= ScaleMax)
               {
                   st.ScaleX = ScaleMax;
                   st.ScaleY = ScaleMax;
                   return;
               }
            st.ScaleX *= ScaleRate;
            st.ScaleY *= ScaleRate;
           }
           else
           {
               if (st.ScaleX / ScaleRate <= ScaleMin)
               {
                   st.ScaleX = ScaleMin;
                   st.ScaleY = ScaleMin;
                    return;
               }
                   
            st.ScaleX /= ScaleRate;
            st.ScaleY /= ScaleRate;
           }
        }


    }
}
