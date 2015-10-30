namespace LordOfUltima.Units.Units
{
    class RangerUnit : IUnit
    {
        private readonly UnitStats _unitStats;
        private readonly string _imagePath;
        private bool _available;
        private readonly string _name;

        RangerUnit()
        {
            _unitStats = new UnitStats()
            {
                Attack = 20,
                InfanteryDefence = 40,
                CavaleryDefence = 10,
                MagicDefence = 25,
                ArtillerieDefence = 15,
                Space = 1,
                FoodUsage = 3,
                Speed = 20,
                AttackStructure = 20
            };
            _imagePath = "pack://application:,,,/Resources/Images/Units/units_ranger.png";
            _name = "Ranger";
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
