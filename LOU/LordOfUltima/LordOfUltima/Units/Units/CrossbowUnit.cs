namespace LordOfUltima.Units.Units
{
    class CrossbowUnit : IUnit
    {
        private readonly UnitStats _unitStats;
        private readonly string _imagePath;
        private bool _available;
        private readonly string _name;

        CrossbowUnit()
        {
            _unitStats = new UnitStats()
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
            _imagePath = "pack://application:,,,/Resources/Images/Units/units_crossbow.png";
            _name = "Crossbowman";
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
