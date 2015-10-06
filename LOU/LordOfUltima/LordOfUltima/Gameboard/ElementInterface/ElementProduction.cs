namespace LordOfUltima.MGameboard
{
    public class ElementProduction
    {
        public ElementProduction(int wood, int stone, int iron, int food, int gold = 0, int research = 0)
        {
            Wood = wood;
            Stone = stone;
            Iron = iron;
            Food = food;
            Gold = gold;
            Research = research;
        }

        // Wood production of Element
        private int _woodProduction;
        public int Wood
        {
            get { return _woodProduction; }
            set { _woodProduction = value; }
        }

        // Stone production of Element
        private int _stoneProduction;
        public int Stone
        {
            get { return _stoneProduction; }
            set { _stoneProduction = value; }
        }

        // Iron production of Element
        private int _ironProduction;
        public int Iron
        {
            get { return _ironProduction; }
            set { _ironProduction = value; }
        }

        // Food production of Element
        private int _foodProduction;
        public int Food
        {
            get { return _foodProduction; }
            set { _foodProduction = value; }
        }

        // Gold production of Element
        private int _goldProduction;
        public int Gold
        {
            get { return _goldProduction; }
            set { _goldProduction = value; }
        }

        // Research production of Element
        private int _researchProduction;
        public int Research
        {
            get { return _researchProduction; }
            set { _researchProduction = value; }
        }

        public int GetFirstNotNull()
        {
            return (_woodProduction > 0)? _woodProduction: (
                (_stoneProduction > 0)? _stoneProduction : ( 
                (_ironProduction > 0)? _ironProduction : (
                (_foodProduction > 0)? _foodProduction : (
                (_goldProduction > 0)? _goldProduction : (
                (_researchProduction > 0)? _researchProduction : 0
                )))));
        }
    }
}
