using System.Collections.Generic;
using LordOfUltima.Research;

namespace LordOfUltima.MGameboard
{
    class TrainingGroundBuilding : IElementType, IUnitBuilding
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

        public TrainingGroundBuilding()
        {
            _elementType = ElementType.Type.BUILDING_TRAINING_GROUND;
            _imagePath = "pack://application:,,,/Resources/Images/Military/building_training_ground.png";
            _detailImagePath = "pack://application:,,,/Resources/Images/Military/menu_training_ground.png";
            _name = "Training Ground";
            _elementInfo = "Allows recruitment of infantry units and, if upgraded, increases their stats.";
            _hasLevelEnable = true;
            _isRessources = false;
            _isBarrack = false;

            // Element cost for each level
            _elementCostsList.Add(1, new ElementCost(20, 40, 0));
            _elementCostsList.Add(2, new ElementCost(40, 80, 0));
            _elementCostsList.Add(3, new ElementCost(80, 160, 0));
            _elementCostsList.Add(4, new ElementCost(200, 400, 0));
            _elementCostsList.Add(5, new ElementCost(600, 1200, 0));
            _elementCostsList.Add(6, new ElementCost(1400, 2800, 0));
            _elementCostsList.Add(7, new ElementCost(2800, 5600, 0));
            _elementCostsList.Add(8, new ElementCost(4800, 9600, 0));
            _elementCostsList.Add(9, new ElementCost(7500, 15000, 0));
            _elementCostsList.Add(10, new ElementCost(11500, 23000, 0));

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