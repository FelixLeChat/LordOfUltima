using System.Collections.Generic;

namespace LordOfUltima.MGameboard
{
    public class MillElementType : IElementType
    {
        private readonly string _name;
        private readonly string _imagePath;
        private readonly string _detailImagePath;
        private readonly string _elementInfo;
        private readonly bool _hasLevelEnable;
        private readonly bool _isRessources;
        private readonly ElementType.Type _elementType;
        private readonly Dictionary<int, ElementCost> _elementCostsList = new Dictionary<int, ElementCost>();
        private readonly Dictionary<int, ElementProductionBonus> _elementProductionBonusList = new Dictionary<int, ElementProductionBonus>();
        private readonly Dictionary<int, int> _elementScoreList = new Dictionary<int, int>(); 

        public MillElementType()
        {
            _elementType = ElementType.Type.BUILDING_MILL;
            _name = ElementType.GetTypeName(_elementType);
            _isRessources = false;
            _hasLevelEnable = true;
            _imagePath = "pack://application:,,,/Resources/Images/building_mill.png";
            _detailImagePath = "pack://application:,,,/Resources/Images/menu_mill.png";
            _elementInfo = "It increases the efficiency of bordering Farms and food storage capacity of any bordering Storage.";

            // Element cost for each level
            _elementCostsList.Add(1, new ElementCost(60, 60, 0));
            _elementCostsList.Add(2, new ElementCost(150, 150, 0));
            _elementCostsList.Add(3, new ElementCost(350, 350, 0));
            _elementCostsList.Add(4, new ElementCost(1100, 1100, 0));
            _elementCostsList.Add(5, new ElementCost(2700, 2700, 0));
            _elementCostsList.Add(6, new ElementCost(5000, 5000, 0));
            _elementCostsList.Add(7, new ElementCost(8500, 8500, 0));
            _elementCostsList.Add(8, new ElementCost(13500, 13500, 0));
            _elementCostsList.Add(9, new ElementCost(21500, 21500, 0));
            _elementCostsList.Add(10, new ElementCost(33000, 33000, 0));

            for (int i = 1; i <= 10; i++)
            {
                _elementProductionBonusList.Add(i, new ElementProductionBonus(0, 0, 30 + (i - 1) * 5, 0));
            }

            // Element Score for each level
            _elementScoreList.Add(1, 1);
            _elementScoreList.Add(2, 4);
            _elementScoreList.Add(3, 7);
            _elementScoreList.Add(4, 14);
            _elementScoreList.Add(5, 24);
            _elementScoreList.Add(6, 36);
            _elementScoreList.Add(7, 50);
            _elementScoreList.Add(8, 68);
            _elementScoreList.Add(9, 90);
            _elementScoreList.Add(10, 120);
        }

        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public bool IsRessources() { return _isRessources; }
        public string GetImagePath() { return _imagePath; }
        public string GetDetailImagePath() { return _detailImagePath; }
        public string GetElementInfo() { return _elementInfo; }
        public ElementType.Type GetElementType() { return _elementType; }
        public ElementProduction GetElementProduction(int level) { return null; }
        public bool IsMilitary() { return false; }

        public ElementCost GetElementCost(int level)
        {
            if (level > 0 && level <= 10)
            {
                return _elementCostsList[level];
            }
            return null;
        }

        public ElementProductionBonus GetElementProductionBonus(int level)
        {
            if (level > 0 && level <= 10)
            {
                return _elementProductionBonusList[level];
            }
            return null;
        }

        public int GetScoreValue(int level)
        {
            if (level > 0 && level <= 10)
            {
                return _elementScoreList[level];
            }
            return 0;
        }

        public ElementStorage GetElementStorage(int level) { return null; }
    }
}