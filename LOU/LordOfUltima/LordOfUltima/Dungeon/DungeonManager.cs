using System;
using System.Collections.Generic;
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
        }
    }
}
