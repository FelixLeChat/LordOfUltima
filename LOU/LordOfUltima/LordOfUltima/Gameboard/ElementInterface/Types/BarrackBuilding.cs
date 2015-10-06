using System;
using System.Collections.Generic;
using LordOfUltima.Research;

namespace LordOfUltima.MGameboard
{
    class BarrackBuilding : IElementType, IUnitBuilding
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
        private readonly Dictionary<int, int> _elementArmySizeList = new Dictionary<int, int>();
        private readonly Dictionary<int, ElementCost> _elementCostsList = new Dictionary<int, ElementCost>();

        public BarrackBuilding()
        {
            _elementType = ElementType.Type.BUILDING_BARRACK;
            _imagePath = "pack://application:,,,/Resources/Images/Military/building_barrack.png";
            _detailImagePath = "pack://application:,,,/Resources/Images/Military/menu_barrack.png";
            _name = "Barrack";
            _elementInfo = "Barrack increases the maximum army size of a city.";
            _hasLevelEnable = true;
            _isRessources = false;
            _isBarrack = true;

            // Element cost for each level
            _elementCostsList.Add(1, new ElementCost(0, 50, 0));
            _elementCostsList.Add(2, new ElementCost(0, 150, 0));
            _elementCostsList.Add(3, new ElementCost(0, 300, 0));
            _elementCostsList.Add(4, new ElementCost(0, 600, 0));
            _elementCostsList.Add(5, new ElementCost(0, 1200, 0));
            _elementCostsList.Add(6, new ElementCost(0, 2500, 0));
            _elementCostsList.Add(7, new ElementCost(0, 4000, 0));
            _elementCostsList.Add(8, new ElementCost(0, 7000, 0));
            _elementCostsList.Add(9, new ElementCost(0, 11500, 0));
            _elementCostsList.Add(10, new ElementCost(0, 17500, 0));

            // Element Score for each level
            _elementScoreList.Add(1, 1);
            _elementScoreList.Add(2, 2);
            _elementScoreList.Add(3, 4);
            _elementScoreList.Add(4, 7);
            _elementScoreList.Add(5, 12);
            _elementScoreList.Add(6, 18);
            _elementScoreList.Add(7, 25);
            _elementScoreList.Add(8, 34);
            _elementScoreList.Add(9, 45);
            _elementScoreList.Add(10, 60);

            // Army size for each level
            _elementArmySizeList.Add(1, 10);
            _elementArmySizeList.Add(2, 30);
            _elementArmySizeList.Add(3, 60);
            _elementArmySizeList.Add(4, 100);
            _elementArmySizeList.Add(5, 160);
            _elementArmySizeList.Add(6, 240);
            _elementArmySizeList.Add(7, 350);
            _elementArmySizeList.Add(8, 500);
            _elementArmySizeList.Add(9, 700);
            _elementArmySizeList.Add(10, 1000);
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
        public int GetUnitBonus(int level) { return 0; }
        public bool IsMilitary() { return true; }

        public int GetArmySize(int level)
        {
            if (level > 0 && level <= 10)
            {
                return _elementArmySizeList[level];
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
