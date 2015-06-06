namespace LordOfUltima.RessourcesProduction
{
    class Ressources
    {
        private static Ressources _ins;
        public static Ressources Instance
        {
            get { return _ins ?? (_ins = new Ressources()); }
        }

        private Ressources() { }

        public double WoodQty { get; set; }

        public double StoneQty { get; set; }

        public double IronQty { get; set; }

        public double FoodQty { get; set; }

        public void Initialise()
        {
            WoodQty = 0;
            StoneQty = 0;
            IronQty = 0;
            FoodQty = 0;
        }
    }
}
