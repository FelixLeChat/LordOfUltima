namespace LordOfUltima.Events
{
    class LevelIndicatorVisibility
    {
        private static LevelIndicatorVisibility _instance;
        public static LevelIndicatorVisibility Instance
        {
            get { return _instance ?? (_instance = new LevelIndicatorVisibility()); }
        }

        private readonly Gameboard _gameboard;
        private MainWindow _mainWindow;
        private LevelIndicatorVisibility()
        {
            _gameboard = Gameboard.getInstance();
            _mainWindow = MainWindow.MIns;
        }

        public void HandleLevelIndicatorVisibility()
        {
            _mainWindow = MainWindow.MIns;
            if (_mainWindow == null)
                return;

            if (_mainWindow.trigger_level.IsChecked)
            {
                ShowLevelIndicator();
            }
            else
            {
                HideLevelIndicator();
            }
        }

        public void HideLevelIndicator()
        {
            foreach (Element element in _gameboard.getMap())
            {
                element.hideLevelIndicator();
            }
        }

        public void ShowLevelIndicator()
        {
            foreach (Element element in _gameboard.getMap())
            {
                element.showLevelIndicator();
            }
        }
    }
}
