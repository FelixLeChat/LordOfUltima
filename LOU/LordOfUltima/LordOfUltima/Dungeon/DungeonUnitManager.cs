using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LordOfUltima.Dungeon
{
    class DungeonUnitManager
    {
        private static DungeonUnitManager _ins;
        public static DungeonUnitManager Instance
        {
            get { return _ins ?? (_ins = new DungeonUnitManager()); }
        }

        public ImageBrush GetUnitImage(DungeonUnitType unitType)
        {
            var imgUrl = "";

            switch (unitType)
            {
                case DungeonUnitType.Spider:
                    imgUrl = "";
                    break;
                case DungeonUnitType.Thief:
                    imgUrl = "";
                    break;
                case DungeonUnitType.Centaur:
                    imgUrl = "";
                    break;
                case DungeonUnitType.Troll:
                    imgUrl = "";
                    break;

                case DungeonUnitType.Skeleton:
                    imgUrl = "";
                    break;
                case DungeonUnitType.Ghoul:
                    imgUrl = "";
                    break;
                case DungeonUnitType.Gargoyle:
                    imgUrl = "";
                    break;
                case DungeonUnitType.Daemon:
                    imgUrl = "";
                    break;

                case DungeonUnitType.Orc:
                    imgUrl = "";
                    break;
                case DungeonUnitType.Troglodyte:
                    imgUrl = "";
                    break;
                case DungeonUnitType.Ettin:
                    imgUrl = "";
                    break;
                case DungeonUnitType.Minotaur:
                    imgUrl = "";
                    break;
            }

            return new ImageBrush { ImageSource = new BitmapImage(new Uri(@imgUrl, UriKind.RelativeOrAbsolute)) };
        }
    }
}
