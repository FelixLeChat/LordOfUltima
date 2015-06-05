namespace LordOfUltima.RessourcesProduction
{
    class Ressources
    {
        private static Ressources m_ins;
        public static Ressources Instance
        {
            get
            {
                if (m_ins == null)
                {
                    m_ins = new Ressources();
                }
                return m_ins;
            }
        }

        private Ressources() { }

        private int _woodQty;
        public int WoodQty
        {
            get { return _woodQty; }
            set { _woodQty = value; }
        }

        private int _stoneQty;
        public int StoneQty
        {
            get { return _stoneQty; }
            set { _stoneQty = value; }
        }

        private int _ironQty;
        public int IronQty
        {
            get { return _ironQty; }
            set { _ironQty = value; }
        }

        private int _foodQty;
        public int FoodQty
        {
            get { return _foodQty; }
            set { _foodQty = value; }
        }
    }
}
