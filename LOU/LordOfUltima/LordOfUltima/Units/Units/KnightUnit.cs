namespace LordOfUltima.Units.Units
{
    class KnightUnit : IUnit
    {
        private readonly UnitStats _unitStats;
        private readonly string _imagePath;
        private bool _available;
        private readonly string _name;

        KnightUnit()
        {
            _unitStats = new UnitStats()
            {
                Attack = 90,
                InfanteryDefence = 40,
                CavaleryDefence = 30,
                MagicDefence = 20,
                ArtillerieDefence = 40,
                Space = 2,
                FoodUsage = 25,
                Speed = 10,
                AttackStructure = 90
            };
            _imagePath = "pack://application:,,,/Resources/Images/Units/units_knight.png";
            _name = "Knight";
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
