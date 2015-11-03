namespace LordOfUltima.Units.Units
{
    class WarlockUnit : Unit
    {
        public WarlockUnit()
        {
            UnitStats = new UnitStats()
            {
                Attack = 120,
                InfanteryDefence = 30,
                CavaleryDefence = 20,
                MagicDefence = 50,
                ArtillerieDefence = 40,
                Space = 2,
                FoodUsage = 20,
                Speed = 10,
                AttackStructure = 120
            };
            ImagePath = "pack://application:,,,/Resources/Images/Units/units_warlock.png";
            Name = "Warlock";
            UnitType = UnitType.Magic;
            UnitEntity = UnitEntity.Warlock;
        }
    }
}
