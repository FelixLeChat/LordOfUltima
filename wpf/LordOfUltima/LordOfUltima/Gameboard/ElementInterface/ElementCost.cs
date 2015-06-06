namespace LordOfUltima.MGameboard
{
    public class ElementCost
    {
        // Parameter constructor
        public ElementCost(int wood, int stone, int iron, int food = 0)
        {
            Wood = wood;
            Stone = stone;
            Iron = iron;
            Food = food;
        }

        // Wood Value of construction
        private int _wood = -1;
        public int Wood
        {
          get { return _wood; }
            set
            {
                if (_wood < 0)
                {
                    _wood = value;
                }
            }
        }

        // Stone value of construction
        private int _stone = -1;
        public int Stone
        {
            get { return _stone; }
            set
            {
                if (_stone < 0)
                {
                    _stone = value;
                }
            }
        }

        // Iron value of construction
        private int _iron = -1;
        public int Iron
        {
            get { return _iron; }
            set
            {
                if (_iron < 0)
                {
                    _iron = value;
                }
            }
        }

        // Food value of construction
        private int _food = -1;
        public int Food
        {
            get { return _food; }
            set
            {
                if (_food < 0)
                {
                    _food = value;
                }
            }
        }

        // Level related to the element
        private int _level = -1;
        public int Level
        {
            get { return _level; }
            set
            {
                if (_level < 0)
                {
                    _level = value;
                }
            }
        }
    }
}
