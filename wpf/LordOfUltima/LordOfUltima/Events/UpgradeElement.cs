using LordOfUltima.Error;
using LordOfUltima.MGameboard;

namespace LordOfUltima.Events
{
    class UpgradeElement
    {
        private static UpgradeElement _instance;
        public static UpgradeElement Instance
        {
            get { return _instance ?? (_instance = new UpgradeElement()); }
        }

        private Element _elementToUpgrade;
        public Element ElementToUpgrade
        {
            set { _elementToUpgrade = value; }
        }

        public bool Upgrade()
        {
            if (BuyElement.Instance.Buy(_elementToUpgrade))
            {
                _elementToUpgrade.Level = _elementToUpgrade.Level + 1;

                // Update all map for ressources
                RessourcesBuildingCheck.Instance.cheakAllNeighbourRessources();

                // Update count (Town Hall)
                BuildingCount.Instance.CountBuildings();

                // Update Score
                int level = _elementToUpgrade.Level;
                IElementType elementType = _elementToUpgrade.GetElementType();
                Score.Score.Instance.ScoreValue += (elementType.GetScoreValue(level) -
                                                    elementType.GetScoreValue(level - 1));
                return true;
            }
            ErrorManager.Instance.AddError(new Error.Error(){Description = Error.Error.Type.NOT_ENOUGH_RESSOURCES_UPGRADE});
            return false;
        }
    }
}
