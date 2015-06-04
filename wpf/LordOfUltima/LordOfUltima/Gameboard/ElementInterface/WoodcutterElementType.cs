using System.Collections.Generic;

namespace LordOfUltima.MGameboard
{
    public class WoodcutterElementType : IElementType
    {
        private readonly string _name;
        private readonly string _imagePath;
        private readonly string _detailImagePath;
        private readonly string _elementInfo;
        private readonly bool _hasLevelEnable;
        private readonly bool _isRessources;
        private readonly ElementType.type _elementType;
        private readonly Dictionary<int,ElementCost> _elementCostsList = new Dictionary<int, ElementCost>();
        private readonly Dictionary<int, ElementProduction> _elementProductionList = new Dictionary<int, ElementProduction>();

        public WoodcutterElementType()
        {
            _elementType = ElementType.type.BUILDING_WOODCUTTER;
            _name = ElementType.getTypeName(_elementType);
            _isRessources = false;
            _hasLevelEnable = true;
            _imagePath = "Media/building/building_woodcutter.png";
            _detailImagePath = "Media/menu/menu_woodcutter.png";
            _elementInfo = "It produces wood. Any bordering woods, Cottages and a maximum of one Sawmill will increase productivity.";

            // Element cost for each level
            _elementCostsList.Add(1,new ElementCost(50,0,0));
            _elementCostsList.Add(2, new ElementCost(200,0,0));
            _elementCostsList.Add(3, new ElementCost(400,200,0));
            _elementCostsList.Add(4,new ElementCost(1400,600,0));
            _elementCostsList.Add(5, new ElementCost(3500, 1500, 0));
            _elementCostsList.Add(6, new ElementCost(6000, 3000, 0));
            _elementCostsList.Add(7, new ElementCost(10000, 5000, 0));
            _elementCostsList.Add(8, new ElementCost(16000, 8000, 0));
            _elementCostsList.Add(9, new ElementCost(25000, 13000, 0));
            _elementCostsList.Add(10, new ElementCost(38000, 20000, 0));

            // Element production for each level
            _elementProductionList.Add(1, new ElementProduction(20, 0, 0, 0));
            _elementProductionList.Add(2, new ElementProduction(40, 0, 0, 0));
            _elementProductionList.Add(3, new ElementProduction(60, 0, 0, 0));
            _elementProductionList.Add(4, new ElementProduction(85, 0, 0, 0));
            _elementProductionList.Add(5, new ElementProduction(110, 0, 0, 0));
            _elementProductionList.Add(6, new ElementProduction(140, 0, 0, 0));
            _elementProductionList.Add(7, new ElementProduction(175, 0, 0, 0));
            _elementProductionList.Add(8, new ElementProduction(210, 0, 0, 0));
            _elementProductionList.Add(9, new ElementProduction(250, 0, 0, 0));
            _elementProductionList.Add(10, new ElementProduction(300, 0, 0, 0));
        }

        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public bool IsRessources() { return _isRessources; }
        public string getImagePath() { return _imagePath; }
        public string getDetailImagePath() { return _detailImagePath; }
        public string GetElementInfo() { return _elementInfo; }
        public ElementType.type GetElementType() { return _elementType; }

        public ElementCost GetElementCost(int level)
        {
            if (level > 0 && level <= 10)
            {
                return _elementCostsList[level];
            }
            return null;
        }

        public ElementProduction GetElementProduction(int level)
        {
            if (level > 0 && level <= 10)
            {
                return _elementProductionList[level];
            }
            return null;
        }
    }
}