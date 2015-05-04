using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace LordOfUltima
{
    class Element
    {
        public Element()
        {
            // Caracteristiques pour le rectangle
            m_rect = new Rectangle();
            m_rect.Width = m_width;
            m_rect.Height = m_height;

            // Images
            m_imgbrush = new ImageBrush();
            //m_imgbrush.ImageSource = new BitmapImage(new Uri(@"Media/none.png", UriKind.Relative));
            m_imgbrush.ImageSource = new BitmapImage(new Uri(@"Media/building/building_iron_quary.png", UriKind.Relative));
            m_rect.Fill = m_imgbrush;

            m_rect.AddHandler(Rectangle.MouseLeftButtonDownEvent, new RoutedEventHandler(leftButtonDown));

            // Level Rectangle
            m_level_rect = new Rectangle();
            m_level_rect.Width = 8;
            m_level_rect.Height = 8;
            ImageBrush imageLvl = new ImageBrush();
            imageLvl.ImageSource = new BitmapImage(new Uri(@"Media/level_rect.png", UriKind.Relative));
            m_level_rect.Fill = imageLvl;
            m_level_rect.IsHitTestVisible = false;

            // Level Label
            m_level_label = new Label();
            m_level_label.Width = 20;
            m_level_label.Height = 20;
            m_level_label.FontSize = 8;
            m_level_label.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xB3, 0x7B));
            m_level_label.Content = m_level.ToString();
        }

        /*
         * Attribuer une nouvelle image pour l'element
        */
        public void setPath(string path)
        {
            m_path = path;

            if (File.Exists(path))
            {
                m_imgbrush.ImageSource = new BitmapImage(new Uri(@path, UriKind.Relative));
            }
            
        }

        public Rectangle getElement()
        {
            return m_rect;
        }
        public Rectangle getLevelElement()
        {
            return m_level_rect;
        }
        public Label getLevelLabel()
        {
            return m_level_label;
        }

        /*
         * Met invalide l'element (ne sera pas affiche)
        */
        public void setInvalid()
        {
            m_isValid = false;
            m_rect.Opacity = 0;
            m_level_rect.Opacity = 0;
            m_level_label.Opacity = 0;
        }

        /*
         * Ajout d'un evenement sur le clic de l'item
        */
        private bool m_isClicked = false;
        private void leftButtonDown(object sender, RoutedEventArgs e)
        {
            m_isClicked = true;
        }

        private Rectangle m_rect;
        private Rectangle m_level_rect;
        private Label m_level_label;

        ImageBrush m_imgbrush;
        private string m_path = "";
        public int m_width = 40;
        public int m_height = 40;
        private bool m_isValid = true;
        private int m_level = 0;

        public void hideLevelIndicator()
        {
            m_level_rect.Opacity = 0;
            m_level_label.Opacity = 0;
        }
        public void showLevelIndicator()
        {
            if(m_isValid)
            {
                m_level_rect.Opacity = 1;
                m_level_label.Opacity = 1;
            }
        }
    }
}
