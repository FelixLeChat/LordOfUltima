namespace LordOfUltima.Units.Units
{
    class BerserkerUnit : IUnit
    {
        private readonly UnitStats _unitStats;
        private readonly string _imagePath;
        private bool _available;
        private readonly string _name;

        BerserkerUnit()
        {
            _unitStats = new UnitStats()
            {
                Attack = 50,
                InfanteryDefence = 15,
                CavaleryDefence = 12,
                MagicDefence = 10,
                ArtillerieDefence = 15,
                Space = 1,
                FoodUsage = 6,
                Speed = 20,
                AttackStructure = 15
            };
            _imagePath = "pack://application:,,,/Resources/Images/Units/units_berserker.png";
            _name = "Berserker";
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
