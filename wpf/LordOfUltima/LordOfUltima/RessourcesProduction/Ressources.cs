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

        public int WoodQty { get; set; }

        public int StoneQty { get; set; }

        public int IronQty { get; set; }

        public int FoodQty { get; set; }
    }
}
