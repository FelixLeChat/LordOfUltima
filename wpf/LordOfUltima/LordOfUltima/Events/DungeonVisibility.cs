using System.Windows;

namespace LordOfUltima.Events
{
    class DungeonVisibility
    {
        private static DungeonVisibility _instance;
        public static DungeonVisibility Instance => _instance ?? (_instance = new DungeonVisibility());

        public void HideDungeon()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.dungeon_map.Visibility = Visibility.Collapsed;
        }

        public void ShowDungeon()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.dungeon_map.Visibility = Visibility.Visible;
        }
    }
}
