namespace LordOfUltima.Units.Units
{
    class WarlockUnit : IUnit
    {
        private readonly UnitStats _unitStats;
        private readonly string _imagePath;
        private bool _available;
        private readonly string _name;

        WarlockUnit()
        {
            _unitStats = new UnitStats()
            {
                Attack = 120,
                InfanteryDefence = 30,
                CavaleryDefence = 20,
                MagicDefence = 50,
                ArtillerieDefence = 40,
                Space = 2,
                FoodUsage = 20,
                Speed = 10,
                AttackStructure = 120
            };
            _imagePath = "pack://application:,,,/Resources/Images/Units/units_warlock.png";
            _name = "Warlock";
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
