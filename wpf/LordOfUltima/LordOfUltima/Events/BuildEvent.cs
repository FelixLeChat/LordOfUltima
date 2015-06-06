using LordOfUltima.MGameboard;

namespace LordOfUltima.Events
{
    class BuildEvent
    {
        private static IElementType _typeToBuild;
        private static Element _elementToBuild;
        private static BuildEvent _buildEvent;
        private static Gameboard _gameboard;

        public static BuildEvent Instance
        {
            get { return _buildEvent ?? (_buildEvent = new BuildEvent()); }
        }

        private BuildEvent()
        {
            _gameboard = Gameboard.Instance;
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
        public void BuildElement()
        {
            if (_elementToBuild != null && _typeToBuild != null)
            {
                // Increase Level of building
                _elementToBuild.Level = 1;
                // build the building
                _elementToBuild.setElementType(_typeToBuild);

                // if we build a farm, spawn fields around it
                if (_typeToBuild.GetElementType() == ElementType.Type.BUILDING_FARM)
                {
                    RessourcesBuildingCheck.Instance.SpawnFields(_elementToBuild);
                }

                // Update all map for ressources
                RessourcesBuildingCheck.Instance.cheakAllNeighbourRessources();

                // if build sucessfull, show in side menu
                BuildingDetailsVisibility.Instance.SetElementMeduDetail(_elementToBuild);
                BuildingDetailsVisibility.Instance.ShowBuildingDetails();
            }     
        }

    }
}
