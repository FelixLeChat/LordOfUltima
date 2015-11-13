using LordOfUltima.MGameboard;

namespace LordOfUltima.Units.Units
{
    class GuardianUnit : Unit
    {
        public GuardianUnit()
        {
            UnitStats = new UnitStats()
            {
                Attack = 10,
                InfanteryDefence = 30,
                CavaleryDefence = 50,
                MagicDefence = 20,
                ArtillerieDefence = 15,
                Space = 1,
                FoodUsage = 3,
                Speed = 20,
                AttackStructure = 10
            };
            UnitCost = new UnitCost()
            {
                Iron = 140,
                Gold = 40
            };
            ImagePath = "pack://application:,,,/Resources/Images/Units/units_guardian.png";
            Name = "Guardian";
            UnitType = UnitType.Infantery;
            UnitEntity = UnitEntity.Guardian;
        }
    }
}
