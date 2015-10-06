using System.Windows;

namespace LordOfUltima.Events
{
    class BuildingMenuVisibility
    {
        private const int EnglobMenuMin = 400;
        private const int EnglobMenuMax = 840;
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

            ShowBuildingRessourcesMenu();
        }

        public void ShowBuildingRessourcesMenu()
        {
            _mainWindow = MainWindow.MIns;
            if (_mainWindow == null)
                return;

            _mainWindow.building_menu_ressources.Visibility = Visibility.Visible;
            _mainWindow.building_menu_units.Visibility = Visibility.Hidden;
            _mainWindow.building_menu_englob.Height = EnglobMenuMax;
        }

        public void ShowBuildingUnitsMenu()
        {
            _mainWindow = MainWindow.MIns;
            if (_mainWindow == null)
                return;

            _mainWindow.building_menu_ressources.Visibility = Visibility.Hidden;
            _mainWindow.building_menu_units.Visibility = Visibility.Visible;
            _mainWindow.building_menu_englob.Height = EnglobMenuMin + 57; // moment where the englob division is on the full page
        }
    }
}
