using System.Collections.Generic;

namespace LordOfUltima.MGameboard
{
    public class TownHallElementType : IElementType
    {
        private readonly string _name;
        private readonly string _imagePath;
        private readonly string _detailImagePath;
        private readonly string _elementInfo;
        private readonly bool _hasLevelEnable;
        private readonly bool _isRessources;
        private readonly ElementType.Type _elementType;
        private readonly Dictionary<int, ElementCost> _elementCostsList = new Dictionary<int, ElementCost>();
        private readonly Dictionary<int, int> _elementScoreList = new Dictionary<int, int>();
        private readonly Dictionary<int, ElementStorage> _elementStorageList = new Dictionary<int, ElementStorage>(); 


        public TownHallElementType()
        {
            _elementType = ElementType.Type.BUILDING_TOWNHALL;
            _name = ElementType.GetTypeName(_elementType);
            _isRessources = false;
            _hasLevelEnable = true;
            _imagePath = "pack://application:,,,/Resources/Images/building_townhall.png";
            _detailImagePath = "pack://application:,,,/Resources/Images/menu_townhall.png";
            _elementInfo = "It provides basic storage and wood production. Also determines the maximum number of buildings in a city. Only one can be built in each city. Cannot be totally demolished or destroyed.";

            // Element cost for each level
            _elementCostsList.Add(1, new ElementCost(0, 0, 0));
            _elementCostsList.Add(2, new ElementCost(200, 0, 0));
            _elementCostsList.Add(3, new ElementCost(500, 100, 0));
            _elementCostsList.Add(4, new ElementCost(1000, 300, 0));
            _elementCostsList.Add(5, new ElementCost(3000, 1500, 0));
            _elementCostsList.Add(6, new ElementCost(8000, 4000, 0));
            _elementCostsList.Add(7, new ElementCost(15000, 10000, 0));
            _elementCostsList.Add(8, new ElementCost(30000, 25000, 0));
            _elementCostsList.Add(9, new ElementCost(60000, 60000, 0));
            _elementCostsList.Add(10, new ElementCost(120000, 120000, 0));

            // Element Score for each level
            _elementScoreList.Add(1, 3);
            _elementScoreList.Add(2, 9);
            _elementScoreList.Add(3, 18);
            _elementScoreList.Add(4, 36);
            _elementScoreList.Add(5, 60);
            _elementScoreList.Add(6, 90);
            _elementScoreList.Add(7, 126);
            _elementScoreList.Add(8, 171);
            _elementScoreList.Add(9, 225);
            _elementScoreList.Add(10, 300);

            // Element Storage for each level
            _elementStorageList.Add(1, new ElementStorage(5000));
            _elementStorageList.Add(2, new ElementStorage(7000));
            _elementStorageList.Add(3, new ElementStorage(10000));
            _elementStorageList.Add(4, new ElementStorage(15000));
            _elementStorageList.Add(5, new ElementStorage(24000));
            _elementStorageList.Add(6, new ElementStorage(35000));
            _elementStorageList.Add(7, new ElementStorage(50000));
            _elementStorageList.Add(8, new ElementStorage(80000));
            _elementStorageList.Add(9, new ElementStorage(125000));
            _elementStorageList.Add(10, new ElementStorage(175000));
        }

        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public bool IsRessources() { return _isRessources; }
        public string GetImagePath() { return _imagePath; }
        public string GetDetailImagePath() { return _detailImagePath; }
        public string GetElementInfo() { return _elementInfo; }
        public ElementType.Type GetElementType() { return _elementType; }
        public ElementProductionBonus GetElementProductionBonus(int level) { return null; }
        public bool IsMilitary() { return false; }

        public int GetScoreValue(int level)
        {
            if (level > 0 && level <= 10)
            {
                return _elementScoreList[level];
            }
            return 0;
        }

        public ElementStorage GetElementStorage(int level)
        {
            if (level > 0 && level <= 10)
            {
                return _elementStorageList[level];
            }
            return null;
        }

        public ElementProduction GetElementProduction(int level)
        {
            // Town hall has the basic production to 300 Wood
            return new ElementProduction(300,0,0,0);
        }

        public ElementCost GetElementCost(int level)
        {
            if (level > 0 && level <= 10)
            {
                return _elementCostsList[level];
            }
            return null;
        }
    }
}