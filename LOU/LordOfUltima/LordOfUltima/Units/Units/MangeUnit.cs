namespace LordOfUltima.Units.Units
{
    class MageUnit : IUnit
    {
        private readonly UnitStats _unitStats;
        private readonly string _imagePath;
        private bool _available;
        private readonly string _name;

        MageUnit()
        {
            _unitStats = new UnitStats()
            {
                Attack = 75,
                InfanteryDefence = 15,
                CavaleryDefence = 10,
                MagicDefence = 30,
                ArtillerieDefence = 15,
                Space = 1,
                FoodUsage = 5,
                Speed = 20,
                AttackStructure = 75
            };
            _imagePath = "pack://application:,,,/Resources/Images/Units/units_mage.png";
            _name = "Mage";
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
