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
                element.initialise();
            }
            // Default img for townhall
            elementMap[9, 9].setElementType(new TownHallElementType());
            elementMap[9, 9].Level = 1;

            // Init ressources production
            RessourcesManager.Instance.CalculateRessources();
        }
    }
}
