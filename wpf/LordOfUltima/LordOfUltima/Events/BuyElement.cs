using LordOfUltima.MGameboard;
using LordOfUltima.RessourcesProduction;

namespace LordOfUltima.Events
{
    class BuyElement
    {
        private static BuyElement _buyElement;
        public static BuyElement Instance
        { get { return _buyElement ?? (_buyElement = new BuyElement()); } }

        public bool Buy(Element element)
        {
            if (element == null)
                return false;

            int elementLevel = element.Level;
            if (elementLevel < 0 || elementLevel >= 10)
                return false;

            if (element.GetElementType() == null)
                return false;

            ElementCost elementCost = element.GetElementType().GetElementCost(elementLevel + 1);

            if (elementCost == null)
                return false;

            return CheckRessourcesAvailability(elementCost);
        }

        public bool Buy(IElementType elementType, int level)
        {
            if (elementType == null)
                return false;

            ElementCost elementCost = elementType.GetElementCost(level);
            if (elementCost == null)
                return false;

            return CheckRessourcesAvailability(elementCost);
        }

        private bool CheckRessourcesAvailability(ElementCost elementcost)
        {
            Ressources ressources = Ressources.Instance;

            if (ressources.WoodQty >= elementcost.Wood &&
                ressources.StoneQty >= elementcost.Stone &&
                ressources.IronQty >= elementcost.Iron &&
                ressources.FoodQty >= elementcost.Food)
            {
                ressources.WoodQty -= (elementcost.Wood > 0) ? elementcost.Wood : 0;
                ressources.StoneQty -= (elementcost.Stone > 0) ? elementcost.Stone : 0;
                ressources.IronQty -= (elementcost.Iron > 0) ? elementcost.Iron : 0;
                ressources.FoodQty -= (elementcost.Food > 0) ? elementcost.Food : 0;
                return true;
            }
            return false;
        }
    }
}
