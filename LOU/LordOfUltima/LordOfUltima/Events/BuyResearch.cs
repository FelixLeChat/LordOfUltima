using LordOfUltima.Research;
using LordOfUltima.RessourcesProduction;

namespace LordOfUltima.Events
{
    class BuyResearch
    {
        private static BuyResearch _buyResearch;
        public static BuyResearch Instance
        { get{return _buyResearch ?? (_buyResearch = new BuyResearch()); } }

        public bool Buy(IResearchType research)
        {
            if (research == null)
                return false;

            int researchLevel = research.GetLevel();
            if (researchLevel < 0 || researchLevel >= research.GetMaxLevel())
                return false;

            ResearchCost cost = research.GetResearchCost(researchLevel + 1);

            if (cost == null)
                return false;

            return CheckRessourcesAvailability(cost);
        }

        private bool CheckRessourcesAvailability(ResearchCost cost)
        {
            Ressources ressources = Ressources.Instance;

            if (ressources.WoodQty >= cost.Wood &&
                ressources.StoneQty >= cost.Stone &&
                ressources.IronQty >= cost.Iron &&
                ressources.FoodQty >= cost.Food &&
                ressources.GoldQty >= cost.Gold &&
                ressources.ResearchQty >= cost.Research)
            {
                ressources.WoodQty -= (cost.Wood > 0) ? cost.Wood : 0;
                ressources.StoneQty -= (cost.Stone > 0) ? cost.Stone : 0;
                ressources.IronQty -= (cost.Iron > 0) ? cost.Iron : 0;
                ressources.FoodQty -= (cost.Food > 0) ? cost.Food : 0;
                ressources.GoldQty -= (cost.Gold > 0) ? cost.Gold : 0;
                ressources.ResearchQty -= (cost.Research > 0) ? cost.Research : 0;
                return true;
            }
            return false;
        }
    }
}
