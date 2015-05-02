using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private int[,] map;

        /*
         * Constructeur prive pour empecher l'acces au constructeur par
         * une autre classe.
        */
        private Gameboard()
        {
            map = new int[frame_count, frame_count];
            frame_width = (width-start_width)/frame_count;
            frame_height = (height - start_height)/frame_count;


            // Definition de la fonction de callback
            Action<string> callback = s => onClick(s);
            // Souscription a l'evenement de click
            EventDispatcher.getInstance().subcribe((int)EventDispatcher.action.CANEVAS_CLICK, callback);
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

        /*
         * Retourne la frame en X ou Y choisie par le curseur
        */
        public int getXFrame(double x)
        {
            int frame = (int)((x-start_width)/frame_width + 1);
            return frame;
        }
        public int getYFrame(double y)
        {
            int frame = (int)((y - start_height)/frame_height + 1);
            return frame;
        }

        public void onClick(string data)
        {
        }
    }
}
