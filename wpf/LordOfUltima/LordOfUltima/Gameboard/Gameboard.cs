using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;

namespace LordOfUltima
{
    class Gameboard
    {
        private static Gameboard m_ins = null;

        // Dimentions pour la fenetre du jeu
        private double width = 830;
        private double start_width = 55;
        private double height = 560;
        private double start_height = 70;
        
        // Information sur les divisions dans le jeu
        private double frame_width;
        private double frame_height;
        private int frame_count = 19;

        private Element[,] m_map;

        /*
         * Constructeur prive pour empecher l'acces au constructeur par
         * une autre classe.
        */
        private Gameboard()
        {
            frame_width = (width - start_width) / frame_count;
            frame_height = (height - start_height) / frame_count;

            // Initialisation de toutes les cases du jeu
            m_map = new Element[frame_count, frame_count];
            for (int i = 0; i < frame_count; i++)
            {
                for (int j = 0; j < frame_count; j++)
                {
                    m_map[i, j] = new Element();

                    m_map[i, j].getElement().Margin = new Thickness(i*frame_width + start_width, j*frame_height + (start_height-frame_height), 0, 0);
                }
            }
            verifyMap();

            /*
            // Definition de la fonction de callback
            Action<string> callback = s => onClick(s);
            // Souscription a l'evenement de click
            EventDispatcher.getInstance().subcribe((int)EventDispatcher.action.CANEVAS_CLICK, callback);*/
        }

        /* Implementation du patron singleton
         * retourne l'instance de la classe
        */
        public static Gameboard getInstance()
        {
            if( m_ins == null)
            {
                m_ins = new Gameboard();
            }
            return m_ins;
        }

        public Element[,] getMap()
        {
            return m_map;
        }

        /*
         * Retourne la frame en X ou Y choisie par le curseur
        */
        public int getXFrame(double x)
        {
            int frame = (int)((x-start_width)/frame_width);
            return frame;
        }
        public int getYFrame(double y)
        {
            int frame = (int)((y - start_height)/frame_height);
            return frame;
        }

        public void onClick(string data)
        {
        }

        /*
         * Verify each element to see if they are valid
        */
        private void verifyMap()
        {
            // coin haut gauche
            m_map[0, 0].setInvalid(); m_map[0, 1].setInvalid(); m_map[1, 0].setInvalid();
            // coin bas gauche
            m_map[0, 18].setInvalid(); m_map[0, 17].setInvalid(); m_map[1, 18].setInvalid();
            // coin haut droit
            m_map[18, 0].setInvalid(); m_map[17, 0].setInvalid(); m_map[18, 1].setInvalid();
            // coin bas droit
            m_map[18, 18].setInvalid(); m_map[18, 17].setInvalid(); m_map[17, 18].setInvalid();

            for(int i = 0; i < 5; i++)
            {
                m_map[i, 9].setInvalid(); m_map[18 - i, 9].setInvalid();
                m_map[9, i].setInvalid(); m_map[9, 18 - i].setInvalid();
            }

        }
    }
}
