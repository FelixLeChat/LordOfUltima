using LordOfUltima.MGameboard;

namespace LordOfUltima.Events
{
    class DeleteElement
    {
        private static DeleteElement _deleteElement;
        public static DeleteElement Instance
        {
            get { return _deleteElement ?? (_deleteElement = new DeleteElement()); }
        }

        private Element _elementToDelete;
        public Element ElementToDelete
        {
            set { _elementToDelete = value; }
        }

        public void Delete()
        {
            if (_elementToDelete == null || _elementToDelete.GetElementType() == null ||
                _elementToDelete.GetElementType().GetElementType() == ElementType.Type.BUILDING_TOWNHALL) 
                return;


            _elementToDelete.Initialise();

            // set the neighbouring ressources count
            RessourcesBuildingCheck.Instance.cheakAllNeighbourRessources();

            // change building count
            BuildingCount.Instance.CountBuildings();

            // hide menu
            BuildingDetailsVisibility.Instance.HideBuildingDetails();

            // handle level indicator
            LevelIndicatorVisibility.Instance.HideLevelIndicator();
            LevelIndicatorVisibility.Instance.HandleLevelIndicatorVisibility();
        }
    }
}
