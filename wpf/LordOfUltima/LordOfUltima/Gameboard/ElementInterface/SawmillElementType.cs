using System.Collections.Generic;

namespace LordOfUltima.MGameboard
{
    public class SawmillElementType : IElementType
    {
        private readonly string _name;
        private readonly string _imagePath;
        private readonly bool _hasLevelEnable;
        private readonly bool _isRessources;
        private readonly ElementType.type _elementType;
        private readonly Dictionary<int, ElementCost> _elementCostsList = new Dictionary<int, ElementCost>(); 

        public SawmillElementType()
        {
            _elementType = ElementType.type.BUILDING_SAWMILL;
            _name = ElementType.getTypeName(_elementType);
            _isRessources = false;
            _hasLevelEnable = true;
            _imagePath = "Media/building/building_sawmill.png";

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
        }

        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public bool IsRessources() { return _isRessources; }
        public string getImagePath() { return _imagePath; }
        public ElementType.type GetElementType() { return _elementType; }
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