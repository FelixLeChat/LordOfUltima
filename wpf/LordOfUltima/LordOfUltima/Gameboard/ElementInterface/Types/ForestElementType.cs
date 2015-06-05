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
        private readonly ElementType.type _elementType;
        private readonly ElementProductionBonus _elementProductionBonus;

        public ForestElementType()
        {
            _elementType = ElementType.type.RESSOURCE_FOREST;
            _name = ElementType.getTypeName(_elementType);
            _isRessources = true;
            _hasLevelEnable = false;
            var select = Random.Next(1, 5);
            _imagePath = "Media/ressource/resource_wood_" + select.ToString() + ".png";
            _detailImagePath = "Media/menu/menu_forest.png";
            _elementInfo = "Each Woodcutter's Hut gains 50% production efficiency from the first adjacent wood node, then 40% for each after that. The Woodcutter's Hut production efficiency can then be further increased up to 75% by an adjacent Sawmill, and up to 30% by each adjacent Cottage.";

            // Bonus linked to Forest
            _elementProductionBonus = new ElementProductionBonus(0, 0, 0, 0);
            _elementProductionBonus.IsRessourcesBonus = true;
            _elementProductionBonus.FirstBonus = 50;
            _elementProductionBonus.StoneBonus = 40;
        }

        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public bool IsRessources() { return _isRessources; }
        public string getImagePath() { return _imagePath; }
        public string getDetailImagePath() { return _detailImagePath; }
        public string GetElementInfo() { return _elementInfo; }
        public ElementType.type GetElementType() { return _elementType;}
        public ElementCost GetElementCost(int level) { return null;} // Return null because there is no level with ressources
        public ElementProduction GetElementProduction(int level) { return null; }
        public ElementProductionBonus GetElementProductionBonus(int level) { return _elementProductionBonus; }
    }
}
