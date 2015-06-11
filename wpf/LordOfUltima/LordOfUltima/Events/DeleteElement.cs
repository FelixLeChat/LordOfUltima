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

            if (_elementToDelete.GetElementType().GetElementType() == ElementType.Type.BUILDING_FARM)
            {
                resetElementNear(_elementToDelete);
            }

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

        private void resetElementNear(Element element)
        {
            if (element == null || !element.HasElement || element.GetElementType() == null)
                return;

            Element[,] map = Gameboard.Instance.GetMap();
            int frameCount = Gameboard.Instance.GetFrameCount;
            int x = element.PositionX;
            int y = element.PositionY;

            if (y > 0)
            {
                if (x > 0)
                {
                    if (!map[x - 1, y - 1].HasElement)
                    {
                        deleteElement((x - 1), (y - 1));
                    }
                }
                if (!map[x, y - 1].HasElement)
                {
                    deleteElement(x, y - 1);
                }

                if (x < frameCount - 1)
                {
                    if (!map[x + 1, y - 1].HasElement)
                    {
                        deleteElement(x + 1, y - 1);
                    }
                }

            }
            if (y < frameCount - 1)
            {
                if (x > 0)
                {
                    if (!map[x - 1, y + 1].HasElement)
                    {
                        deleteElement(x - 1, y + 1);
                    }
                }

                if (!map[x, y + 1].HasElement)
                {
                    deleteElement(x, y + 1);
                }

                if (x < frameCount - 1)
                {
                    if (!map[x + 1, y + 1].HasElement)
                    {
                        deleteElement(x + 1, y + 1);
                    }
                }
            }

            if (x > 0)
            {
                if (!map[x - 1, y].HasElement)
                {
                    deleteElement(x - 1, y);
                }
            }
            if (x < frameCount - 1)
            {
                if (!map[x + 1, y].HasElement)
                {
                    deleteElement(x + 1, y);
                }
            }
        }

        private void deleteElement(int x, int y)
        {
            Element[,] map = Gameboard.Instance.GetMap();
            map[x, y].Initialise();
        }
    }
}
