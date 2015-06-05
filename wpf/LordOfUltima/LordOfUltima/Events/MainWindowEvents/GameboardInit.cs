namespace LordOfUltima.Events
{
    internal class GameboardInit
    {
        private static GameboardInit _instance;
        public static GameboardInit Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameboardInit();
                }
                return _instance;
            }
        }

        private Gameboard _gameboard;
        private MainWindow _mainWindow;
        private GameboardInit()
        {
            _gameboard = Gameboard.getInstance();
            _mainWindow = MainWindow.m_ins;
        }

        /*
         * Insertion du tableau d'Elements dans le canvas (a l'initialisation de la fenetre)
        */
        public void InsertMap()
        {
            _mainWindow = MainWindow.m_ins;
            if (_mainWindow == null)
                return;

            foreach (Element element in _gameboard.getMap())
            {
                // Add building to canvas
                _mainWindow.canvas1.Children.Add(element.getElement());
                // Add building level to canvas
                _mainWindow.canvas1.Children.Add(element.getLevelElement());
                // Add level label to canvas
                _mainWindow.canvas1.Children.Add(element.getLevelLabel());
                // Add select rect to canvas
                _mainWindow.canvas1.Children.Add(element.getSelectElement());
            }
        }
    }
}
