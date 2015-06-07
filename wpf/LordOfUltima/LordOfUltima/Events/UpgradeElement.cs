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
                return true;
            }
            return false;
        }
    }
}
