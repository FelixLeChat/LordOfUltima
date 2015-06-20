using System.Windows;

namespace LordOfUltima.Events
{
    class BuildingMenuVisibility
    {
        private const int EnglobMenuMin = 400;
        private const int EnglobMenuMax = 800;
        private static BuildingMenuVisibility _instance;
        public static BuildingMenuVisibility Instance
        {
            get { return _instance ?? (_instance = new BuildingMenuVisibility()); }
        }

        private MainWindow _mainWindow;
        private BuildingMenuVisibility()
        {
            _mainWindow = MainWindow.MIns;
        }

        public void HideBuildingMenu()
        {
            _mainWindow = MainWindow.MIns;
            if (_mainWindow == null)
                return;

            _mainWindow.building_menu.Visibility = Visibility.Collapsed;
            _mainWindow.scrollview.ScrollToTop();
            _mainWindow.building_menu_englob.Height = EnglobMenuMin;
        }

        public void ShowBuildingMenu()
        {
            _mainWindow = MainWindow.MIns;
            if (_mainWindow == null)
                return;

            _mainWindow.building_menu.Visibility = Visibility.Visible;
            _mainWindow.scrollview.ScrollToTop();
            _mainWindow.building_menu_englob.Height = EnglobMenuMax;
        }
    }
}
