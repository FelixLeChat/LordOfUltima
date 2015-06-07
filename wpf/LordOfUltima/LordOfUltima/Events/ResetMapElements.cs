using LordOfUltima.MGameboard;
using LordOfUltima.RessourcesProduction;

namespace LordOfUltima.Events
{
    class ResetMapElements
    {
        private static ResetMapElements _ins;
        public static ResetMapElements Instance
        {
            get { return _ins ?? (_ins = new ResetMapElements()); }
        }

        public void ResetMap()
        {
            ResetMapElementBorder.Instance.ResetSelectionBorder();

            Element[,] elementMap = Gameboard.Instance.GetMap();

            foreach (var element in elementMap)
            {
                element.Initialise();
            }

            // Default img for townhall
            elementMap[9, 9].SetElementType(new TownHallElementType());
            elementMap[9, 9].Level = 1;

            // Init ressources production
            RessourcesManager.Instance.CalculateRessources();

            // Init starting ressources
            Ressources.Instance.Initialise();
            Ressources.Instance.WoodQty += 3000;
            Ressources.Instance.StoneQty += 1000;
        }
    }
}
