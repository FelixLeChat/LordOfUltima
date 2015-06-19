using System;

namespace LordOfUltima.MGameboard
{
    public class ForestElementType : IElementType
    {
        private static readonly Random Random = new Random(12221);
        private readonly string _name;
        private readonly string _imagePath;
        private readonly string _detailImagePath;
        private readonly string _elementInfo;
        private readonly bool _hasLevelEnable;
        private readonly bool _isRessources;
        private readonly ElementType.Type _elementType;
        private readonly ElementProductionBonus _elementProductionBonus;

        public ForestElementType()
        {
            _elementType = ElementType.Type.RESSOURCE_FOREST;
            _name = ElementType.GetTypeName(_elementType);
            _isRessources = true;
            _hasLevelEnable = false;
            var select = Random.Next(1, 5);
            _imagePath = "pack://application:,,,/Resources/Images/resource_wood_" + select + ".png";
            _detailImagePath = "pack://application:,,,/Resources/Images/menu_forest.png";
            _elementInfo = "Each Woodcutter's Hut gains 50% production efficiency from the first adjacent wood node, then 40% for each after that. The Woodcutter's Hut production efficiency can then be further increased up to 75% by an adjacent Sawmill, and up to 30% by each adjacent Cottage.";

            // Bonus linked to Forest
            _elementProductionBonus = new ElementProductionBonus(0, 0, 0, 0);
            _elementProductionBonus.IsRessourcesBonus = true;
            _elementProductionBonus.FirstBonus = 50;
            _elementProductionBonus.SecondBonus = 40;
        }

        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public bool IsRessources() { return _isRessources; }
        public string GetImagePath() { return _imagePath; }
        public string GetDetailImagePath() { return _detailImagePath; }
        public string GetElementInfo() { return _elementInfo; }
        public ElementType.Type GetElementType() { return _elementType;}
        public ElementCost GetElementCost(int level) { return null;} // Return null because there is no level with ressources
        public ElementProduction GetElementProduction(int level) { return null; }
        public ElementProductionBonus GetElementProductionBonus(int level) { return _elementProductionBonus; }
    }
}
