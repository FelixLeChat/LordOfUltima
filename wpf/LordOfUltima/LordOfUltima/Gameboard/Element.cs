using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        /*
         * Met invalide l'element (ne sera pas affiche)
        */
        public void setInvalid()
        {
            m_isValid = false;
            m_rect.Opacity = 0;
        }

        private Rectangle m_rect;
        ImageBrush m_imgbrush;
        private string m_path = "";
        public int m_width = 40;
        public int m_height = 40;
        private bool m_isValid = true;
    }
}
