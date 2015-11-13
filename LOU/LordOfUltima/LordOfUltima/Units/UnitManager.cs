using System.Collections.Generic;
using LordOfUltima.MGameboard;
using LordOfUltima.Units.Units;

namespace LordOfUltima.Units
{
    class UnitManager
    {
        private static UnitManager _ins;
        public int TotalUnits;
        public readonly Dictionary<UnitEntity, Unit> Units = new Dictionary<UnitEntity, Unit>();
        public Dictionary<UnitEntity, int> UnitsAvailables = new Dictionary<UnitEntity, int>();

        public static UnitManager Instance
        {
            get { return _ins ?? (_ins = new UnitManager()); }
        }

        private UnitManager()
        {
            Units.Add(UnitEntity.Berserker, new BerserkerUnit());
            Units.Add(UnitEntity.Cityguard, new CityguardUnit());
            Units.Add(UnitEntity.Crossbow, new CrossbowUnit());
            Units.Add(UnitEntity.Guardian, new GuardianUnit());
            Units.Add(UnitEntity.Knight, new KnightUnit());
            Units.Add(UnitEntity.Mage, new MageUnit());
            Units.Add(UnitEntity.Paladin, new PaladinUnit());
            Units.Add(UnitEntity.Ranger, new RangerUnit());
            Units.Add(UnitEntity.Scout, new ScoutUnit());
            Units.Add(UnitEntity.Templar, new TemplarUnit());
            Units.Add(UnitEntity.Warlock, new WarlockUnit());

            foreach (var unit in Units)
            {
                UnitsAvailables.Add(unit.Key, 0);
            }
        }

        public void UpdateUnitCount()
        {
            TotalUnits = 0;
            Element[,] map = Gameboard.Instance.GetMap();

            foreach (var element in map)
            {
                var elementType = element?.GetElementType();

                if (elementType is BarrackBuilding)
                {
                    var level = element.Level;
                    TotalUnits += ((BarrackBuilding) elementType).GetArmySize(level);
                }               
            }

            var mainWindow = MainWindow.MIns;
            if (mainWindow == null)
                return;

            mainWindow.units_count.Content = TotalUnits;
        }
    }
}
