using System.Collections.Generic;
using LordOfUltima.Research;

namespace LordOfUltima.MGameboard
{
    class StableBuilding : IElementType, IUnitBuilding
    {
        private readonly string _imagePath;
        private readonly string _name;
        private readonly string _elementInfo;
        private readonly string _detailImagePath;
        private readonly bool _hasLevelEnable;
        private readonly bool _isRessources;
        private readonly bool _isBarrack;
        private readonly ElementType.Type _elementType;

        private readonly Dictionary<int, int> _elementScoreList = new Dictionary<int, int>();
        private readonly Dictionary<int, ElementCost> _elementCostsList = new Dictionary<int, ElementCost>();

        public StableBuilding()
        {
            _elementType = ElementType.Type.BUILDING_STABLE;
            _imagePath = "pack://application:,,,/Resources/Images/Military/building_stable.png";
            _detailImagePath = "pack://application:,,,/Resources/Images/Military/menu_stable.png";
            _name = "Stable";
            _elementInfo = "Allows recruitment of cavalry units and, if upgraded, increases their stats.";
            _hasLevelEnable = true;
            _isRessources = false;
            _isBarrack = false;

            // Element cost for each level
            _elementCostsList.Add(1, new ElementCost(25, 50, 0));
            _elementCostsList.Add(2, new ElementCost(55, 110, 0));
            _elementCostsList.Add(3, new ElementCost(110, 220, 0));
            _elementCostsList.Add(4, new ElementCost(275, 550, 0));
            _elementCostsList.Add(5, new ElementCost(800, 1600, 0));
            _elementCostsList.Add(6, new ElementCost(1900, 3800, 0));
            _elementCostsList.Add(7, new ElementCost(3750, 7500, 0));
            _elementCostsList.Add(8, new ElementCost(6500, 13000, 0));
            _elementCostsList.Add(9, new ElementCost(10200, 20400, 0));
            _elementCostsList.Add(10, new ElementCost(15500, 31000, 0));

            // Element Score for each level
            _elementScoreList.Add(1, 1);
            _elementScoreList.Add(2, 3);
            _elementScoreList.Add(3, 6);
            _elementScoreList.Add(4, 12);
            _elementScoreList.Add(5, 20);
            _elementScoreList.Add(6, 30);
            _elementScoreList.Add(7, 42);
            _elementScoreList.Add(8, 57);
            _elementScoreList.Add(9, 75);
            _elementScoreList.Add(10, 100);

        }

        public string GetElementInfo() { return _elementInfo; }
        public string GetImagePath() { return _imagePath; }
        public string GetDetailImagePath() { return _detailImagePath; }
        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public ElementProductionBonus GetElementProductionBonus(int level) { return null; }
        public ElementStorage GetElementStorage(int level) { return null; }
        public bool IsRessources() { return _isRessources; }
        public ElementProduction GetElementProduction(int level) { return null; }
        public ElementType.Type GetElementType() { return _elementType; }
        public bool IsBarrack() { return _isBarrack; }
        public int GetArmySize(int level) { return 0; }
        public bool IsMilitary() { return true; }

        public int GetUnitBonus(int level)
        {
            if (level > 0 && level <= 10)
            {
                return level;
            }
            return 0;
        }

        public ElementCost GetElementCost(int level)
        {
            if (level > 0 && level <= 10)
            {
                return _elementCostsList[level];
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
    }
}