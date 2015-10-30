namespace LordOfUltima.Units.Units
{
    class PaladinUnit : IUnit
    {
        private readonly UnitStats _unitStats;
        private readonly string _imagePath;
        private bool _available;
        private readonly string _name;

        PaladinUnit()
        {
            _unitStats = new UnitStats()
            {
                Attack = 60,
                InfanteryDefence = 50,
                CavaleryDefence = 20,
                MagicDefence = 90,
                ArtillerieDefence = 40,
                Space = 2,
                FoodUsage = 15,
                Speed = 10,
                AttackStructure = 60
            };
            _imagePath = "pack://application:,,,/Resources/Images/Units/units_paladin.png";
            _name = "Paladin";
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
