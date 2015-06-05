namespace LordOfUltima.MGameboard
{
    public class ElementProduction
    {
        public ElementProduction(int wood, int stone, int iron, int food)
        {
            Wood = wood;
            Stone = stone;
            Iron = iron;
            Food = food;
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

        public int GetFirstNotNull()
        {
            return (_woodProduction > 0)? _woodProduction: (
                (_stoneProduction > 0)? _stoneProduction : ( 
                (_ironProduction > 0)? _ironProduction : (
                (_foodProduction > 0)? _foodProduction : 0
                    )));
        }
    }
}
