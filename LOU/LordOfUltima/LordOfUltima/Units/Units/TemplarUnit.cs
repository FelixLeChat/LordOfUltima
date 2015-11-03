namespace LordOfUltima.Units.Units
{
    class TemplarUnit : Unit
    {
        public TemplarUnit()
        {
            UnitStats = new UnitStats()
            {
                Attack = 25,
                InfanteryDefence = 20,
                CavaleryDefence = 30,
                MagicDefence = 50,
                ArtillerieDefence = 15,
                Space = 1,
                FoodUsage = 3,
                Speed = 20,
                AttackStructure = 25
            };
            ImagePath = "pack://application:,,,/Resources/Images/Units/units_templar.png";
            Name = "Templar";
            UnitType = UnitType.Infantery;
            UnitEntity = UnitEntity.Templar;
        }
    }
}
