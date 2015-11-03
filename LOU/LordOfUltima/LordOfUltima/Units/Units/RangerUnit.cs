namespace LordOfUltima.Units.Units
{
    class RangerUnit : Unit
    {
        public RangerUnit()
        {
            UnitStats = new UnitStats()
            {
                Attack = 20,
                InfanteryDefence = 40,
                CavaleryDefence = 10,
                MagicDefence = 25,
                ArtillerieDefence = 15,
                Space = 1,
                FoodUsage = 3,
                Speed = 20,
                AttackStructure = 20
            };
            ImagePath = "pack://application:,,,/Resources/Images/Units/units_ranger.png";
            Name = "Ranger";
            UnitType = UnitType.Infantery;
            UnitEntity = UnitEntity.Ranger;
        }
    }
}
