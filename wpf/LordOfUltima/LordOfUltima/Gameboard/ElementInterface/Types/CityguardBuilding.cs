using System;
using System.Collections.Generic;
using LordOfUltima.Research;

namespace LordOfUltima.MGameboard
{
    class CityguardBuilding : IElementType, IUnitBuilding
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

        public CityguardBuilding()
        {
            _elementType = ElementType.Type.BUILDING_CITYGUARD_HOUSE;
            _imagePath = "pack://application:,,,/Resources/Images/Military/building_cityguard_house.png";
            _detailImagePath = "pack://application:,,,/Resources/Images/Military/menu_cityguard_house.png";
            _name = "Barrack";
            _elementInfo = "It allows recruitment of City Guards and, if upgraded, increases their stats.";
            _hasLevelEnable = true;
            _isRessources = false;
            _isBarrack = false;

            // Element cost for each level
            _elementCostsList.Add(1, new ElementCost(15, 30, 0));
            _elementCostsList.Add(2, new ElementCost(30, 60, 0));
            _elementCostsList.Add(3, new ElementCost(55, 110, 0));
            _elementCostsList.Add(4, new ElementCost(140, 280, 0));
            _elementCostsList.Add(5, new ElementCost(400, 800, 0));
            _elementCostsList.Add(6, new ElementCost(1000, 2000, 0));
            _elementCostsList.Add(7, new ElementCost(1900, 3800, 0));
            _elementCostsList.Add(8, new ElementCost(3200, 6400, 0));
            _elementCostsList.Add(9, new ElementCost(5100, 10200, 0));
            _elementCostsList.Add(10, new ElementCost(8000, 16000, 0));

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
