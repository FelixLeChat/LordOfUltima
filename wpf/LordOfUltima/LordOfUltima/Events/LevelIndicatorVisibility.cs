namespace LordOfUltima.Events
{
    class LevelIndicatorVisibility
    {
        private static LevelIndicatorVisibility _instance;
        public static LevelIndicatorVisibility Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LevelIndicatorVisibility();
                }
                return _instance;
            }
        }

        private Gameboard _gameboard;
        private MainWindow _mainWindow;
        private LevelIndicatorVisibility()
        {
            _gameboard = Gameboard.getInstance();
            _mainWindow = MainWindow.m_ins;
        }

        public void handleLevelIndicatorVisibility()
        {
            _mainWindow = MainWindow.m_ins;
            if (_mainWindow == null)
                return;

            if (_mainWindow.trigger_level.IsChecked)
            {
                showLevelIndicator();
            }
            else
            {
                hideLevelIndicator();
            }
        }

        public void hideLevelIndicator()
        {
            foreach (Element element in _gameboard.getMap())
            {
                element.hideLevelIndicator();
            }
        }

        public void showLevelIndicator()
        {
            foreach (Element element in _gameboard.getMap())
            {
                element.showLevelIndicator();
            }
        }
    }
}
