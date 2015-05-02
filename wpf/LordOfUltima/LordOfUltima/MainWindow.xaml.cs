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

            // Background pour canvas
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@"Media/main_square.png", UriKind.Relative));
            canvas1.Background = imageBrush;

            // Set the gameboard Instance
            m_gameboard = Gameboard.getInstance();
            // Insertion des elements dans la carte
            insertMap();
            
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            double mouseX = Mouse.GetPosition(grid).X;
            double mouseY = Mouse.GetPosition(grid).Y;
            mousePosX.Content = "Mouse X : " + mouseX;
            mousePosY.Content = "Mouse Y : " + mouseY;

            frameX.Content = "Frame X : " + m_gameboard.getXFrame(mouseX);
            frameY.Content = "Frame Y : " + m_gameboard.getYFrame(mouseY);
        }

        private void insertMap()
        {
            foreach(Element element in m_gameboard.getMap())
            {
                canvas1.Children.Add(element.getElement());
            }
        }


    }
}
