using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;

namespace LordOfUltima
{
    class Gameboard
    {
        private static Gameboard _ins;
        /* Implementation du patron singleton
         * retourne l'instance de la classe
        */
        public static Gameboard Instance
        {
            get { return _ins ?? (_ins = new Gameboard()); } 
        }

        // Dimentions pour la fenetre du jeu
        private const double Width = 830;
        private const double StartWidth = 55;
        private const double Height = 560;
        private const double StartHeight = 55;
        
        // Information sur les divisions dans le jeu
        private readonly double _frameWidth;
        private readonly double _frameHeight;
        private const int FrameCount = 19;
        public int GetFrameCount
        {
            get { return FrameCount; }
        }

        private readonly Element[,] _map;
        public Element[,] GetMap()
        {
            return _map;
        }
        /*
         * Constructeur prive pour empecher l'acces au constructeur par
         * une autre classe.
        */
        private Gameboard()
        {
            _frameWidth = (Width - StartWidth) / FrameCount;
            _frameHeight = (Height - StartHeight) / FrameCount;

            // Initialisation de toutes les cases du jeu
            _map = new Element[FrameCount, FrameCount];
            for (int i = 0; i < FrameCount; i++)
            {
                for (int j = 0; j < FrameCount; j++)
                {
                    _map[i, j] = new Element
                    {
                        PositionX = i,
                        PositionY = j
                    };

                    //set x and Y
                    Rectangle elementRect = _map[i, j].getElement();

                    // Position for building element
                    elementRect.Margin = new Thickness(i*_frameWidth + StartWidth, j*_frameHeight + (StartHeight-_frameHeight), 0, 0);

                    // Position for building level
                    Rectangle levelRect = _map[i, j].getLevelElement();
                    levelRect.Margin = new Thickness(elementRect.Margin.Left + _frameWidth / 3, elementRect.Margin.Top + _frameHeight, 0, 0);

                    // Position for label level
                    double left = levelRect.Margin.Left;
                    double top = levelRect.Margin.Top;
                    _map[i, j].getLevelLabel().Margin = new Thickness(left-3.1, top-7, 0, 0);

                    // Position for select element
                    Border selectRect = _map[i, j].getSelectElement();
                    selectRect.Margin = new Thickness(i * _frameWidth + StartWidth, j * _frameHeight + (StartHeight - _frameHeight)+5, 0, 0);
                }
            }
        }

        /*
         * Retourne la frame en X ou Y choisie par le curseur
        */
        public int GetXFrame(double x)
        {
            return (int)((x-StartWidth)/_frameWidth);
        }
        public int GetYFrame(double y)
        {
            return (int)((y - StartHeight)/_frameHeight);
        }
    }
}
