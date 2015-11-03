namespace LordOfUltima.Units.Units
{
    class KnightUnit : Unit
    {
        public KnightUnit()
        {
            UnitStats = new UnitStats()
            {
                Attack = 90,
                InfanteryDefence = 40,
                CavaleryDefence = 30,
                MagicDefence = 20,
                ArtillerieDefence = 40,
                Space = 2,
                FoodUsage = 25,
                Speed = 10,
                AttackStructure = 90
            };
            ImagePath = "pack://application:,,,/Resources/Images/Units/units_knight.png";
            Name = "Knight";
            UnitType = UnitType.Cavalery;
            UnitEntity = UnitEntity.Knight;
        }
    }
}
