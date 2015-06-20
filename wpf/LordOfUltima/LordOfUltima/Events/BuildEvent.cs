using LordOfUltima.Error;
using LordOfUltima.MGameboard;

namespace LordOfUltima.Events
{
    class BuildEvent
    {
        private static IElementType _typeToBuild;
        private static Element _elementToBuild;
        private static BuildEvent _buildEvent;

        public static BuildEvent Instance
        {
            get { return _buildEvent ?? (_buildEvent = new BuildEvent()); }
        }

        public void SetTypeToBuild(IElementType elementType)
        {
            _typeToBuild = elementType;
        }

        public void SetElementToBuild(Element element)
        {
            _elementToBuild = element;
        }

        // Here is the place where we build the element for the first time(on map)
        public bool BuildElement()
        {
            if (_elementToBuild != null && _typeToBuild != null)
            {
                BuildingCount buildingCount = BuildingCount.Instance;
                if (buildingCount.Count >= buildingCount.MaxCount)
                {
                    ErrorManager.Instance.AddError(new Error.Error(){Description = Error.Error.Type.NOT_ENOUGH_BUILDING_SPACE});
                    return false;
                }

                if (BuyElement.Instance.Buy(_typeToBuild, 1))
                {
                    // Increase Level of building
                    _elementToBuild.Level = 1;
                    // build the building
                    _elementToBuild.SetElementType(_typeToBuild);

                    // if we build a farm, spawn fields around it
                    if (_typeToBuild.GetElementType() == ElementType.Type.BUILDING_FARM)
                    {
                        RessourcesBuildingCheck.Instance.SpawnFields(_elementToBuild);
                    }

                    // Update all map for ressources
                    RessourcesBuildingCheck.Instance.cheakAllNeighbourRessources();

                    // change building count
                    BuildingCount.Instance.CountBuildings();

                    // if build sucessfull, show in side menu
                    BuildingDetailsVisibility.Instance.SetElementMenuDetail(_elementToBuild);
                    BuildingDetailsVisibility.Instance.ShowBuildingDetails();

                    // Set the element to upgrade for next level
                    UpgradeElement.Instance.ElementToUpgrade =_elementToBuild;
                    DeleteElement.Instance.ElementToDelete = _elementToBuild;

                    // Update score
                    Score.Score.Instance.ScoreValue += _elementToBuild.GetElementType().GetScoreValue(1);
                  
                    return true;
                }
                else
                {
                    ErrorManager.Instance.AddError(new Error.Error() { Description = Error.Error.Type.NOT_ENOUGH_RESSOURCES_BUILD });
                }
            }
            return false;
        }

    }
}
