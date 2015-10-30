namespace LordOfUltima.Units.Units
{
    class CityguardUnit : IUnit
    {
        private readonly UnitStats _unitStats;
        private readonly string _imagePath;
        private bool _available;
        private readonly string _name;

        CityguardUnit()
        {
            _unitStats = new UnitStats()
            {
                Attack = 10,
                InfanteryDefence = 10,
                CavaleryDefence = 10,
                MagicDefence = 10,
                ArtillerieDefence = 10,
                Space = 0,
                FoodUsage = 2,
                Speed = 0,
                AttackStructure = 0
            };
            _imagePath = "pack://application:,,,/Resources/Images/Units/units_cityguard.png";
            _name = "Cityguard";
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
