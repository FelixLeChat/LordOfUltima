namespace LordOfUltima.Units.Units
{
    class ScoutUnit : Unit
    {
        public ScoutUnit()
        {
            UnitStats = new UnitStats()
            {
                Attack = 10,
                InfanteryDefence = 10,
                CavaleryDefence = 10,
                MagicDefence = 10,
                ArtillerieDefence = 10,
                Space = 2,
                FoodUsage = 5,
                Speed = 8,
                AttackStructure = 10
            };
            UnitCost = new UnitCost()
            {
                Iron = 40,
                Gold = 120
            };
            ImagePath = "pack://application:,,,/Resources/Images/Units/units_scout.png";
            Name = "Scout";
            UnitType = UnitType.Cavalery;
            UnitEntity = UnitEntity.Scout;
        }
    }
}
