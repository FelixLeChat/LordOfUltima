namespace LordOfUltima.Units.Units
{
    class CityguardUnit : Unit
    {
        public CityguardUnit()
        {
            UnitStats = new UnitStats()
            {
                Attack = 10,
                InfanteryDefence = 10,
                CavaleryDefence = 10,
                MagicDefence = 10,
                ArtillerieDefence = 10,
                Space = 1,
                FoodUsage = 2,
                Speed = 0,
                AttackStructure = 0
            };
            UnitCost = new UnitCost()
            {
                Wood = 100
            };
            ImagePath = "pack://application:,,,/Resources/Images/Units/units_cityguard.png";
            Name = "Cityguard";
            UnitType = UnitType.Infantery;
            UnitEntity = UnitEntity.Cityguard;
        }
    }
}
