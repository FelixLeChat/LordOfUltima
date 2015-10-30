namespace LordOfUltima.Units.Units
{
    class GuardianUnit : IUnit
    {
        private readonly UnitStats _unitStats;
        private readonly string _imagePath;
        private bool _available;
        private readonly string _name;

        GuardianUnit()
        {
            _unitStats = new UnitStats()
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
            _imagePath = "pack://application:,,,/Resources/Images/Units/units_guardian.png";
            _name = "Guardian";
        }
        public UnitStats GetUnitStats()
        {
            return _unitStats;
        }

        public string GetImagePath()
        {
            return _imagePath;
        }

        public string GetName()
        {
            return _name;
        }

        public bool GetAvailable()
        {
            return _available;
        }

        public void SetAvailability(bool availability)
        {
            _available = availability;
        }
    }
}
