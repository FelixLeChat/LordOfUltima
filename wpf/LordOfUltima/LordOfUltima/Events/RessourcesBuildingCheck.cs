using LordOfUltima.MGameboard;
using LordOfUltima.RessourcesProduction;

namespace LordOfUltima.Events
{
    class RessourcesBuildingCheck
    {
        private static RessourcesBuildingCheck _instance;
        public static RessourcesBuildingCheck Instance
        {
            get { return _instance ?? (_instance = new RessourcesBuildingCheck()); }
        }

        private readonly Gameboard _gameboard;
        private RessourcesBuildingCheck()
        {
            _gameboard = Gameboard.Instance;
        }

        private void checkNeighbourRessources(Element element)
        {
            if (element == null || !element.HasElement || element.GetElementType() == null)
                return;

            Element[,] map = _gameboard.GetMap();
            int frameCount = _gameboard.GetFrameCount;
            int x = element.PositionX;
            int y = element.PositionY;
            int ressourceCount = 0;

            ElementType.Type ressourceType = ElementType.GetBonusRessource(element.GetElementType().GetElementType());

            if (ressourceType == ElementType.Type.DEFAULT)
            {
                return;
            }


            if (y > 0)
            {
                if (x > 0)
                {
                    if (map[x - 1, y - 1].HasElement && map[x - 1, y - 1].GetElementType().GetElementType() == ressourceType)
                        ressourceCount++;
                }
                if (map[x, y - 1].HasElement && map[x, y - 1].GetElementType().GetElementType() == ressourceType)
                    ressourceCount++;

                if (x < frameCount - 1)
                {
                    if (map[x + 1, y - 1].HasElement && map[x + 1, y - 1].GetElementType().GetElementType() == ressourceType)
                        ressourceCount++;
                }

            }
            if (y < frameCount - 1)
            {
                if (x > 0)
                {
                    if (map[x - 1, y + 1].HasElement && map[x - 1, y + 1].GetElementType().GetElementType() == ressourceType)
                        ressourceCount++;
                }

                if (map[x, y + 1].HasElement && map[x, y + 1].GetElementType().GetElementType() == ressourceType)
                    ressourceCount++;

                if (x < frameCount - 1)
                {
                    if (map[x + 1, y + 1].HasElement && map[x + 1, y + 1].GetElementType().GetElementType() == ressourceType)
                        ressourceCount++;
                }
            }

            if (x > 0)
            {
                if (map[x - 1, y].HasElement && map[x - 1, y].GetElementType().GetElementType() == ressourceType)
                    ressourceCount++;
            }
            if (x < frameCount - 1)
            {
                if (map[x + 1, y].HasElement && map[x + 1, y].GetElementType().GetElementType() == ressourceType)
                    ressourceCount++;
            }

            element.NbRessourcesAround = ressourceCount;

        }

        public void cheakAllNeighbourRessources()
        {
            Element[,] map = _gameboard.GetMap();
            int frameCount = _gameboard.GetFrameCount;

            for (int i = 0; i < frameCount; i++)
            {
                for (int j = 0; j < frameCount; j++)
                {
                    checkNeighbourRessources(map[i, j]);
                    checkNeighbourBonusBuilding(map[i, j]);
                    CheckFieldsCount(map[i, j]);

                    // Update ressource production
                    RessourcesManager.Instance.CalculateRessources();
                }
            }
        }

        public void SpawnFields(Element element)
        {
            if (element == null || !element.HasElement || element.GetElementType() == null)
                return;

            Element[,] map = _gameboard.GetMap();
            int frameCount = _gameboard.GetFrameCount;
            int x = element.PositionX;
            int y = element.PositionY;

            if (y > 0)
            {
                if (x > 0)
                {
                    if (!map[x - 1, y - 1].HasElement)
                    {
                        buildFields((x - 1), (y - 1));
                    }
                }
                if (!map[x, y - 1].HasElement)
                {
                    buildFields(x, y - 1);
                }

                if (x < frameCount - 1)
                {
                    if (!map[x + 1, y - 1].HasElement)
                    {
                        buildFields(x + 1, y - 1);
                    }
                }

            }
            if (y < frameCount - 1)
            {
                if (x > 0)
                {
                    if (!map[x - 1, y + 1].HasElement)
                    {
                        buildFields(x - 1, y + 1);
                    }
                }

                if (!map[x, y + 1].HasElement)
                {
                    buildFields(x, y + 1);
                }

                if (x < frameCount - 1)
                {
                    if (!map[x + 1, y + 1].HasElement)
                    {
                        buildFields(x + 1, y + 1);
                    }
                }
            }

            if (x > 0)
            {
                if (!map[x - 1, y].HasElement)
                {
                    buildFields(x - 1, y);
                }
            }
            if (x < frameCount - 1)
            {
                if (!map[x + 1, y].HasElement)
                {
                    buildFields(x + 1, y);
                }
            }
        }

        private void buildFields(int x, int y)
        {
            Element[,] map = _gameboard.GetMap();
            map[x, y].setElementType(new FieldsElementType());
            map[x, y].HasElement = false;
        }

        public void CheckFieldsCount(Element element)
        {
            if (element == null || !element.HasElement || element.GetElementType() == null)
                return;

            if (element.GetElementType().GetElementType() != ElementType.Type.BUILDING_FARM)
                return;

            Element[,] map = _gameboard.GetMap();
            int frameCount = _gameboard.GetFrameCount;
            int x = element.PositionX;
            int y = element.PositionY;
            int fieldsCount = 0;

            #region Fields count
            if (y > 0)
            {
                if (x > 0)
                {
                    if (!map[x - 1, y - 1].HasElement)
                    {
                        fieldsCount++;
                    }
                }
                if (!map[x, y - 1].HasElement)
                {
                    fieldsCount++;
                }

                if (x < frameCount - 1)
                {
                    if (!map[x + 1, y - 1].HasElement)
                    {
                        fieldsCount++;
                    }
                }

            }
            if (y < frameCount - 1)
            {
                if (x > 0)
                {
                    if (!map[x - 1, y + 1].HasElement)
                    {
                        fieldsCount++;
                    }
                }

                if (!map[x, y + 1].HasElement)
                {
                    fieldsCount++;
                }

                if (x < frameCount - 1)
                {
                    if (!map[x + 1, y + 1].HasElement)
                    {
                        fieldsCount++;
                    }
                }
            }

            if (x > 0)
            {
                if (!map[x - 1, y].HasElement)
                {
                    fieldsCount++;
                }
            }
            if (x < frameCount - 1)
            {
                if (!map[x + 1, y].HasElement)
                {
                    fieldsCount++;
                }
            }
            #endregion

            element.FieldsCount = fieldsCount;
        }

        private void checkNeighbourBonusBuilding(Element element)
        {
            if (element == null || !element.HasElement || element.GetElementType() == null)
                return;

            Element[,] map = _gameboard.GetMap();
            int frameCount = _gameboard.GetFrameCount;
            int x = element.PositionX;
            int y = element.PositionY;
            Element bonusBuilding = null;

            ElementType.Type buildingBonurType =
                ElementType.GetBonusBuilding(element.GetElementType().GetElementType());

            if (buildingBonurType == ElementType.Type.DEFAULT)
            {
                return;
            }


            if (y > 0)
            {
                if (x > 0)
                {
                    if (map[x - 1, y - 1].HasElement &&
                        map[x - 1, y - 1].GetElementType().GetElementType() == buildingBonurType)
                    {
                        bonusBuilding = getBestOne(bonusBuilding, map[x - 1, y - 1]);
                    }
                        
                }
                if (map[x, y - 1].HasElement && map[x, y - 1].GetElementType().GetElementType() == buildingBonurType)
                {
                    bonusBuilding = getBestOne(bonusBuilding, map[x, y - 1]);
                }
                    

                if (x < frameCount - 1)
                {
                    if (map[x + 1, y - 1].HasElement &&
                        map[x + 1, y - 1].GetElementType().GetElementType() == buildingBonurType)
                    {
                        bonusBuilding = getBestOne(bonusBuilding, map[x + 1, y - 1]);
                    }
                        
                }

            }
            if (y < frameCount - 1)
            {
                if (x > 0)
                {
                    if (map[x - 1, y + 1].HasElement &&
                        map[x - 1, y + 1].GetElementType().GetElementType() == buildingBonurType)
                    {
                        bonusBuilding = getBestOne(bonusBuilding, map[x - 1, y + 1]);
                    }
                        
                }

                if (map[x, y + 1].HasElement && map[x, y + 1].GetElementType().GetElementType() == buildingBonurType)
                {
                    bonusBuilding = getBestOne(bonusBuilding, map[x, y + 1]);
                }
                    

                if (x < frameCount - 1)
                {
                    if (map[x + 1, y + 1].HasElement &&
                        map[x + 1, y + 1].GetElementType().GetElementType() == buildingBonurType)
                    {
                        bonusBuilding = getBestOne(bonusBuilding, map[x + 1, y + 1]);
                    }
                        
                }
            }

            if (x > 0)
            {
                if (map[x - 1, y].HasElement && map[x - 1, y].GetElementType().GetElementType() == buildingBonurType)
                {
                    bonusBuilding = getBestOne(bonusBuilding, map[x - 1, y]);
                }
                    
            }
            if (x < frameCount - 1)
            {
                if (map[x + 1, y].HasElement && map[x + 1, y].GetElementType().GetElementType() == buildingBonurType)
                {
                    bonusBuilding = getBestOne(bonusBuilding, map[x + 1, y]);
                }
                    
            }

            element.BonusBuilding = bonusBuilding;
        }

        private Element getBestOne(Element element1, Element element2)
        {
            if (element1 == null)
            {
                return element2;
            }
            if (element2 == null)
            {
                return element1;
            }

            return (element1.Level > element2.Level) ? element1 : element2;
        }
    }
}
