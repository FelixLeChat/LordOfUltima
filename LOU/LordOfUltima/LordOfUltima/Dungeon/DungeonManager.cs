using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LordOfUltima.Units.Units;

namespace LordOfUltima.Dungeon
{
    class DungeonManager
    {
        private static DungeonManager _ins;
        public static DungeonManager Instance
        {
            get { return _ins ?? (_ins = new DungeonManager()); }
        }

        private readonly Dictionary<DungeonType, Dictionary<int, List<Tuple<DungeonUnitType, int>>>> _dungeonContent = new Dictionary<DungeonType, Dictionary<int, List<Tuple<DungeonUnitType, int>>>>();

        private DungeonManager()
        {
            _dungeonContent.Add(DungeonType.Hill, new Dictionary<int, List<Tuple<DungeonUnitType, int>>>());
            _dungeonContent[DungeonType.Hill].Add(1, new List<Tuple<DungeonUnitType, int>>());
            _dungeonContent[DungeonType.Hill].Add(2, new List<Tuple<DungeonUnitType, int>>());
            _dungeonContent[DungeonType.Hill].Add(3, new List<Tuple<DungeonUnitType, int>>());

            _dungeonContent.Add(DungeonType.Mountain, new Dictionary<int, List<Tuple<DungeonUnitType, int>>>());
            _dungeonContent[DungeonType.Mountain].Add(1, new List<Tuple<DungeonUnitType, int>>());
            _dungeonContent[DungeonType.Mountain].Add(2, new List<Tuple<DungeonUnitType, int>>());
            _dungeonContent[DungeonType.Mountain].Add(3, new List<Tuple<DungeonUnitType, int>>());

            _dungeonContent.Add(DungeonType.Forest, new Dictionary<int, List<Tuple<DungeonUnitType, int>>>());
            _dungeonContent[DungeonType.Forest].Add(1, new List<Tuple<DungeonUnitType, int>>());
            _dungeonContent[DungeonType.Forest].Add(2, new List<Tuple<DungeonUnitType, int>>());
            _dungeonContent[DungeonType.Forest].Add(3, new List<Tuple<DungeonUnitType, int>>());
        }

        public void OpenDungeon(DungeonType dungeonType, int dungeonLevel)
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.dungeon_attack_page.Visibility = Visibility.Visible;
            mainWindow.dungeon_type.Content = dungeonType.ToString();
            mainWindow.dungeon_level.Content = dungeonLevel;
            SetImageDungeon(dungeonType, mainWindow.dungeon_image);
        }

        public void CloseDungeon()
        {
            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.dungeon_attack_page.Visibility = Visibility.Collapsed;
        }

        private void SetImageDungeon(DungeonType dungeonType, Rectangle rect)
        {
            var imgUrl = "pack://application:,,,/Resources/Images/Dungeons/Npccamp_";

            switch (dungeonType)
            {
                case DungeonType.Forest:
                    imgUrl += "woods";
                    break;
                case DungeonType.Mountain:
                    imgUrl += "mountains";
                    break;
                case DungeonType.Hill:
                    imgUrl += "hill";
                    break;
            }

            imgUrl += "_active.png";
            rect.Fill = new ImageBrush {ImageSource = new BitmapImage(new Uri(@imgUrl, UriKind.RelativeOrAbsolute))};
        }
    }
}
