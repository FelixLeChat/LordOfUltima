namespace LordOfUltima.Units.Units
{
    class MageUnit : Unit
    {
        public MageUnit()
        {
            UnitStats = new UnitStats()
            {
                Attack = 75,
                InfanteryDefence = 15,
                CavaleryDefence = 10,
                MagicDefence = 30,
                ArtillerieDefence = 15,
                Space = 1,
                FoodUsage = 5,
                Speed = 20,
                AttackStructure = 75
            };
            ImagePath = "pack://application:,,,/Resources/Images/Units/units_mage.png";
            Name = "Mage";
            UnitType = UnitType.Magic;
            UnitEntity = UnitEntity.Mage;
        }
    }
}
