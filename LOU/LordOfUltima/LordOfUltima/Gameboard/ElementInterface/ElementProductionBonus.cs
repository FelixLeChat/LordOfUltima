namespace LordOfUltima.MGameboard
{
    public class ElementProductionBonus
    {
        public ElementProductionBonus(int wood, int stone, int iron, int food)
        {
            IsRessourcesBonus = false;
            WoodBonus = wood;
            StoneBonus = stone;
            IronBonus = iron;
            FoodBonus = food;
        }

        private int _woodProductionBonus;
        public int WoodBonus
        {
            get { return _woodProductionBonus; }
            set
            {
                if (checkIfValid(value))
                {
                    _woodProductionBonus = value;
                }
            }
        }

        private int _stoneProductionBonus;
        public int StoneBonus
        {
            get { return _stoneProductionBonus; }
            set
            {
                if (checkIfValid(value))
                {
                    _stoneProductionBonus = value;
                }
            }
        }

        private int _ironProductionBonus;
        public int IronBonus
        {
            get { return _ironProductionBonus; }
            set
            {
                if (checkIfValid(value))
                {
                    _ironProductionBonus = value;
                }
            }
        }

        private int _foodProductionBonus;
        public int FoodBonus
        {
            get { return _foodProductionBonus; }
            set
            {
                if (checkIfValid(value))
                {
                    _foodProductionBonus = value;
                }
            }
        }

        public int GetFirstNotNull()
        {
            return (_woodProductionBonus > 0) ? _woodProductionBonus : (
                (_stoneProductionBonus > 0) ? _stoneProductionBonus : (
                (_ironProductionBonus > 0) ? _ironProductionBonus : (
                (_foodProductionBonus > 0) ? _foodProductionBonus : 0
                    )));
        }

        private bool checkIfValid(int value )
        {
            if (value >= 0 && value <= 100)
            {
                return true;
            }
            return false;
        }


        // Indicate if the ressources is from Ressources (no level)
        public bool IsRessourcesBonus { get; set; }

        private int _firstBonus;
        public int FirstBonus
        {
            get { return _firstBonus; }
            set
            {
                if (checkIfValid(value))
                {
                    _firstBonus = value;
                }
            }
        }

        private int _secondBonus;
        public int SecondBonus
        {
            get { return _secondBonus; }
            set
            {
                if (checkIfValid(value))
                {
                    _secondBonus = value;
                }
            }
        }
    }
}
