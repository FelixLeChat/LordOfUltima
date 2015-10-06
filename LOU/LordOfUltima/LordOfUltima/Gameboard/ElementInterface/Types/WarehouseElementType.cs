using System.Collections.Generic;

namespace LordOfUltima.MGameboard
{
    public class WarehouseElementType : IElementType
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

        public WarehouseElementType()
        {
            _elementType = ElementType.Type.BUILDING_WAREHOUSE;
            _name = ElementType.GetTypeName(_elementType);
            _isRessources = false;
            _hasLevelEnable = true;
            _imagePath = "pack://application:,,,/Resources/Images/building_storage.png";
            _detailImagePath = "pack://application:,,,/Resources/Images/menu_storage.png";
            _elementInfo = "It increases the storage capacity for all resources. Enhanced by any bordering Sawmill, Stonemason, Mill or Foundry.";

            // Element cost for each level
            _elementCostsList.Add(1, new ElementCost(60, 0, 0));
            _elementCostsList.Add(2, new ElementCost(150, 0, 0));
            _elementCostsList.Add(3, new ElementCost(250, 50, 0));
            _elementCostsList.Add(4, new ElementCost(500, 150, 0));
            _elementCostsList.Add(5, new ElementCost(1600, 400, 0));
            _elementCostsList.Add(6, new ElementCost(3000, 1000, 0));
            _elementCostsList.Add(7, new ElementCost(6000, 2000, 0));
            _elementCostsList.Add(8, new ElementCost(9600, 4800, 0));
            _elementCostsList.Add(9, new ElementCost(15000, 9000, 0));
            _elementCostsList.Add(10, new ElementCost(20000, 13000, 0));


            // Element Score for each level
            _elementScoreList.Add(1, 1);
            _elementScoreList.Add(2, 2);
            _elementScoreList.Add(3, 5);
            _elementScoreList.Add(4, 10);
            _elementScoreList.Add(5, 16);
            _elementScoreList.Add(6, 24);
            _elementScoreList.Add(7, 34);
            _elementScoreList.Add(8, 46);
            _elementScoreList.Add(9, 60);
            _elementScoreList.Add(10, 80);

            // Element Storage for each level
            _elementStorageList.Add(1, new ElementStorage(2500));
            _elementStorageList.Add(2, new ElementStorage(5000));
            _elementStorageList.Add(3, new ElementStorage(9000));
            _elementStorageList.Add(4, new ElementStorage(16000));
            _elementStorageList.Add(5, new ElementStorage(26000));
            _elementStorageList.Add(6, new ElementStorage(42000));
            _elementStorageList.Add(7, new ElementStorage(65000));
            _elementStorageList.Add(8, new ElementStorage(100000));
            _elementStorageList.Add(9, new ElementStorage(145000));
            _elementStorageList.Add(10, new ElementStorage(200000));
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

        public ElementCost GetElementCost(int level)
        {
            if (level > 0 && level <= 10)
            {
                return _elementCostsList[level];
            }
            return null;
        }

        public ElementProduction GetElementProduction(int level){ return null; }
    }
}
