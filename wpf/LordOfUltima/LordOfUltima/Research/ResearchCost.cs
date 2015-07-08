using LordOfUltima.MGameboard;

namespace LordOfUltima.Research
{
    class ResearchCost
    {
        public ResearchCost(){}

        // Parameter constructor
        public ResearchCost(int gold, int research, ElementCost elementCost = null)
        {
            if (elementCost != null)
            {
                Wood = elementCost.Wood;
                Stone = elementCost.Stone;
                Iron = elementCost.Iron;
                Food = elementCost.Food;                   
            }

            Gold = gold;
            Research = research;
        }

        // Research Value
        private int _research = -1;
        public int Research
        {
            get { return _research; }
            set
            {
                if (_research < 0)
                {
                    _research = value;
                }
            }
        }

        // Gold Value of Research
        private int _gold = -1;
        public int Gold
        {
            get { return _gold; }
            set
            {
                if (_gold < 0)
                {
                    _gold = value;
                }
            }
        }

        // Wood Value of Research
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

        // Stone value of Research
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

        // Iron value of Research
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

        // Food value of Research
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

        // Level related to the Research
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
