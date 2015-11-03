namespace LordOfUltima.Units.Units
{
    class CrossbowUnit : Unit
    {
        public CrossbowUnit()
        {
            UnitStats = new UnitStats()
            {
                Attack = 40,
                InfanteryDefence = 40,
                CavaleryDefence = 90,
                MagicDefence = 30,
                ArtillerieDefence = 40,
                Space = 2,
                FoodUsage = 10,
                Speed = 10,
                AttackStructure = 40
            };
            ImagePath = "pack://application:,,,/Resources/Images/Units/units_crossbow.png";
            Name = "Crossbowman";
            UnitType = UnitType.Cavalery;
            UnitEntity = UnitEntity.Crossbow;
        }
    }
}
