namespace LordOfUltima.Units.Units
{
    class ScoutUnit : IUnit
    {
        private readonly UnitStats _unitStats;
        private readonly string _imagePath;
        private bool _available;
        private readonly string _name;

        ScoutUnit()
        {
            _unitStats = new UnitStats()
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
            _imagePath = "pack://application:,,,/Resources/Images/Units/units_scout.png";
            _name = "Scout";
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
