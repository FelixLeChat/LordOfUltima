namespace LordOfUltima.Events
{
    internal class GameboardInit
    {
        private static GameboardInit _instance;
        public static GameboardInit Instance
        {
            get { return _instance ?? (_instance = new GameboardInit()); }
        }

        private readonly Gameboard _gameboard;
        private MainWindow _mainWindow;
        private GameboardInit()
        {
            _gameboard = Gameboard.Instance;
            _mainWindow = MainWindow.MIns;
        }

        /*
         * Insertion du tableau d'Elements dans le canvas (a l'initialisation de la fenetre)
        */
        public void InsertMap()
        {
            _mainWindow = MainWindow.MIns;
            if (_mainWindow == null)
                return;

            foreach (Element element in _gameboard.GetMap())
            {
                // Add building to canvas
                _mainWindow.canvas1.Children.Add(element.GetElement());
                // Add building level to canvas
                _mainWindow.canvas1.Children.Add(element.GetLevelElement());
                // Add level label to canvas
                _mainWindow.canvas1.Children.Add(element.GetLevelLabel());
                // Add select rect to canvas
                _mainWindow.canvas1.Children.Add(element.GetSelectElement());
            }
        }
    }
}
