using System.Collections.Generic;

namespace LordOfUltima.MGameboard
{
    public class MarketplaceElementType : IElementType
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

        public MarketplaceElementType()
        {
            _elementType = ElementType.Type.BUILDING_MARKETPLACE;
            _name = ElementType.GetTypeName(_elementType);
            _isRessources = false;
            _hasLevelEnable = true;
            _imagePath = "pack://application:,,,/Resources/Images/building_marketplace.png";
            _detailImagePath = "pack://application:,,,/Resources/Images/menu_marketplace.png";
            _elementInfo = "It allows transport of resources between cities and enables trade with other players by land. Also increases the tax income of connected buildings.";

            // Element cost for each level
            _elementCostsList.Add(1, new ElementCost(40, 20, 0));
            _elementCostsList.Add(2, new ElementCost(80, 40, 0));
            _elementCostsList.Add(3, new ElementCost(160, 80, 0));
            _elementCostsList.Add(4, new ElementCost(400, 200, 0));
            _elementCostsList.Add(5, new ElementCost(1200, 600, 0));
            _elementCostsList.Add(6, new ElementCost(2800, 1400, 0));
            _elementCostsList.Add(7, new ElementCost(5600, 2800, 0));
            _elementCostsList.Add(8, new ElementCost(9600, 4800, 0));
            _elementCostsList.Add(9, new ElementCost(15200, 7600, 0));
            _elementCostsList.Add(10, new ElementCost(23200, 11600, 0));

            for (int i = 1; i <= 10; i++)
            {
                _elementProductionBonusList.Add(i, new ElementProductionBonus(0, 0, i*2, 0));
            }

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
        }

        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public bool IsRessources() { return _isRessources; }
        public string GetImagePath() { return _imagePath; }
        public string GetDetailImagePath() { return _detailImagePath; }
        public string GetElementInfo() { return _elementInfo; }
        public ElementType.Type GetElementType() { return _elementType; }
        public ElementProduction GetElementProduction(int level) { return null; }
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
    }
}