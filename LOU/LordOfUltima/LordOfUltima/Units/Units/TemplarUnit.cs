namespace LordOfUltima.Units.Units
{
    class TemplarUnit : IUnit
    {
        private readonly UnitStats _unitStats;
        private readonly string _imagePath;
        private bool _available;
        private readonly string _name;

        TemplarUnit()
        {
            _unitStats = new UnitStats()
            {
                Attack = 25,
                InfanteryDefence = 20,
                CavaleryDefence = 30,
                MagicDefence = 50,
                ArtillerieDefence = 15,
                Space = 1,
                FoodUsage = 3,
                Speed = 20,
                AttackStructure = 25
            };
            _imagePath = "pack://application:,,,/Resources/Images/Units/units_templar.png";
            _name = "Templar";
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
