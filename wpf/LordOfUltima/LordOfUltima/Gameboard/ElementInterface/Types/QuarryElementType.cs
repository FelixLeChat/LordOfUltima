using System.Collections.Generic;

namespace LordOfUltima.MGameboard
{
    public class QuarryElementType : IElementType
    {
        private readonly string _name;
        private readonly string _imagePath;
        private readonly string _detailImagePath;
        private readonly string _elementInfo;
        private readonly bool _hasLevelEnable;
        private readonly bool _isRessources;
        private readonly ElementType.Type _elementType;
        private readonly Dictionary<int, ElementCost> _elementCostsList = new Dictionary<int, ElementCost>();
        private readonly Dictionary<int, ElementProduction> _elementProductionList = new Dictionary<int, ElementProduction>();

        public QuarryElementType()
        {
            _elementType = ElementType.Type.BUILDING_QUARRY;
            _name = ElementType.GetTypeName(_elementType);
            _isRessources = false;
            _hasLevelEnable = true;
            _imagePath = "pack://application:,,,/Resources/Images/building_stone_quary.png";
            _detailImagePath = "pack://application:,,,/Resources/Images/menu_quarry.png";
            _elementInfo = "It produces Stone. Productivity can be increased by bordering Rocks, Cottages and a maximum of one Stonemason.";

            // Element cost for each level
            _elementCostsList.Add(1, new ElementCost(50, 0, 0));
            _elementCostsList.Add(2, new ElementCost(200, 0, 0));
            _elementCostsList.Add(3, new ElementCost(400, 200, 0));
            _elementCostsList.Add(4, new ElementCost(1400, 600, 0));
            _elementCostsList.Add(5, new ElementCost(3500, 1500, 0));
            _elementCostsList.Add(6, new ElementCost(6000, 3000, 0));
            _elementCostsList.Add(7, new ElementCost(10000, 5000, 0));
            _elementCostsList.Add(8, new ElementCost(16000, 8000, 0));
            _elementCostsList.Add(9, new ElementCost(25000, 13000, 0));
            _elementCostsList.Add(10, new ElementCost(38000, 20000, 0));

            // Element production for each level
            _elementProductionList.Add(1, new ElementProduction(0, 20, 0, 0));
            _elementProductionList.Add(2, new ElementProduction(0, 40, 0, 0));
            _elementProductionList.Add(3, new ElementProduction(0, 60, 0, 0));
            _elementProductionList.Add(4, new ElementProduction(0, 85, 0, 0));
            _elementProductionList.Add(5, new ElementProduction(0, 110, 0, 0));
            _elementProductionList.Add(6, new ElementProduction(0, 140, 0, 0));
            _elementProductionList.Add(7, new ElementProduction(0, 175, 0, 0));
            _elementProductionList.Add(8, new ElementProduction(0, 210, 0, 0));
            _elementProductionList.Add(9, new ElementProduction(0, 250, 0, 0));
            _elementProductionList.Add(10, new ElementProduction(0, 300, 0, 0));
        }

        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public bool IsRessources() { return _isRessources; }
        public string GetImagePath() { return _imagePath; }
        public string GetDetailImagePath() { return _detailImagePath; }
        public string GetElementInfo() { return _elementInfo; }
        public ElementType.Type GetElementType() { return _elementType; }
        public ElementProductionBonus GetElementProductionBonus(int level) { return null; }

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