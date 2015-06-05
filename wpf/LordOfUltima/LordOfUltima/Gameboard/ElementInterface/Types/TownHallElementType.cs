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
        private readonly ElementType.type _elementType;
        private readonly Dictionary<int, ElementCost> _elementCostsList = new Dictionary<int, ElementCost>();

        public TownHallElementType()
        {
            _elementType = ElementType.type.BUILDING_TOWNHALL;
            _name = ElementType.getTypeName(_elementType);
            _isRessources = false;
            _hasLevelEnable = true;
            _imagePath = "Media/building/building_townhall.png";
            _detailImagePath = "Media/menu/menu_townhall.png";
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
        }

        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public bool IsRessources() { return _isRessources; }
        public string getImagePath() { return _imagePath; }
        public string getDetailImagePath() { return _detailImagePath; }
        public string GetElementInfo() { return _elementInfo; }
        public ElementType.type GetElementType() { return _elementType; }
        public ElementProductionBonus GetElementProductionBonus(int level) { return null; }

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