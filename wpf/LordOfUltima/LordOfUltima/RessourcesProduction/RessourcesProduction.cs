using System;
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
        }

        public int WoodQty { get; set; }

        public int StoneQty { get; set; }

        public int IronQty { get; set; }

        public int FoodQty { get; set; }
    }
}
