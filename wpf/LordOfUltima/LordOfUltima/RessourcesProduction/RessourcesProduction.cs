namespace LordOfUltima.RessourcesProduction
{
    class RessourcesProduction
    {
        private static RessourcesProduction _ins;
        public static RessourcesProduction Instance
        {
            get { return _ins ?? (_ins = new RessourcesProduction()); }
        }

        private RessourcesProduction() { }

        public void InitRessourcesProduction()
        {
            WoodQty = 0;
            StoneQty = 0;
            IronQty = 0;
            FoodQty = 0;
            GoldQty = 0;
        }

        public double WoodQty { get; set; }

        public double StoneQty { get; set; }

        public double IronQty { get; set; }

        public double FoodQty { get; set; }

        public double GoldQty { get; set; }
    }
}
