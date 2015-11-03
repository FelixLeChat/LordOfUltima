namespace LordOfUltima.Units.Units
{
    class PaladinUnit : Unit
    {
        public PaladinUnit()
        {
            UnitStats = new UnitStats()
            {
                Attack = 60,
                InfanteryDefence = 50,
                CavaleryDefence = 20,
                MagicDefence = 90,
                ArtillerieDefence = 40,
                Space = 2,
                FoodUsage = 15,
                Speed = 10,
                AttackStructure = 60
            };
            ImagePath = "pack://application:,,,/Resources/Images/Units/units_paladin.png";
            Name = "Paladin";
            UnitType = UnitType.Cavalery;
            UnitEntity = UnitEntity.Paladin;
        }
    }
}
