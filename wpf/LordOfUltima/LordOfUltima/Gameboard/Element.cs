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
            m_rect.IsEnabled = true;

            m_rect.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(leftButtonDown);
            m_rect.MouseLeftButtonUp += new MouseButtonEventHandler(leftButtonUp);


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
            m_level_label.IsHitTestVisible = false;

            // Click rect
            m_click_border = new Border();
            m_click_border.Width = m_width;
            m_click_border.Height = m_height;
            int borderThickness = 2;
            m_click_border.BorderThickness = new Thickness(borderThickness, borderThickness, borderThickness, borderThickness);
            m_click_border.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xC9, 0xD6, 0x3A));
            m_click_border.Visibility = Visibility.Hidden;
            m_click_border.IsHitTestVisible = false;
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
        public Border getSelectElement()
        {
            return m_click_border;
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
        private void leftButtonUp(object sender, RoutedEventArgs e)
        {
            if(m_isClicked)
            {
                m_isClicked = false;
                Gameboard.getInstance().resetSelectionBorder();

                if(m_isValid)
                    showSelectBorder();
            }
        }

        // Elements graphiques lie au building
        private Rectangle m_rect;
        private Rectangle m_level_rect;
        private Border m_click_border;
        private Label m_level_label;


        ImageBrush m_imgbrush;
        private string m_path = "";
        public int m_width = 40;
        public int m_height = 40;
        private bool m_isValid = true;
        private int m_level = 0;

        /*
         * Methode pour la gestion de la presence de l'indicateur de niveau
        */
        public bool m_hasLevelIndicator = false;
        public void hideLevelIndicator()
        {
            m_level_rect.Opacity = 0;
            m_level_label.Opacity = 0;
        }
        public void showLevelIndicator()
        {
            // Verifier si l'objet est valide (dois etre afficher) avant de l'afficher
            if(m_isValid)
            {
                m_level_rect.Opacity = 1;
                m_level_label.Opacity = 1;
            }
        }

        public bool m_hasSelectBorder = false;
        public void hideSelectBorder()
        {
            m_click_border.Visibility = Visibility.Hidden;
        }
        public void showSelectBorder()
        {
            m_click_border.Visibility = Visibility.Visible;
        }
    }
}
