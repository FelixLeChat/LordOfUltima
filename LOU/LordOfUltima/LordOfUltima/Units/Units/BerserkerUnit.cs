namespace LordOfUltima.Units.Units
{
    class BerserkerUnit : Unit
    {
        public BerserkerUnit()
        {
            UnitStats = new UnitStats()
            {
                Attack = 50,
                InfanteryDefence = 15,
                CavaleryDefence = 12,
                MagicDefence = 10,
                ArtillerieDefence = 15,
                Space = 1,
                FoodUsage = 6,
                Speed = 20,
                AttackStructure = 15
            };
            UnitCost = new UnitCost()
            {
                Iron = 150
            };
            ImagePath = "pack://application:,,,/Resources/Images/Units/units_berserker.png";
            Name = "Berserker";
            UnitType = UnitType.Infantery;
            UnitEntity = UnitEntity.Berserker;
        }

    }
}
