using System.Windows;

namespace LordOfUltima.Events
{
    class BuildingMenuVisibility
    {
        private static BuildingMenuVisibility _instance;
        public static BuildingMenuVisibility Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BuildingMenuVisibility();
                }
                return _instance;
            }
        }

        private MainWindow _mainWindow;
        private BuildingMenuVisibility()
        {
            _mainWindow = MainWindow.m_ins;
        }

        public void hideBuildingMenu()
        {
            _mainWindow = MainWindow.m_ins;
            if (_mainWindow == null)
                return;

            _mainWindow.building_menu.Visibility = Visibility.Collapsed;
            _mainWindow.scrollview.ScrollToTop();
            _mainWindow.building_menu_englob.Height = 400;
        }

        public void showBuildingMenu()
        {
            _mainWindow = MainWindow.m_ins;
            if (_mainWindow == null)
                return;

            _mainWindow.building_menu.Visibility = Visibility.Visible;
            _mainWindow.scrollview.ScrollToTop();
            _mainWindow.building_menu_englob.Height = 600;
        }
    }
}
