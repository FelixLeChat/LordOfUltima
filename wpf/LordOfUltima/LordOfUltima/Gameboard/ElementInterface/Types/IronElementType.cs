using System;

namespace LordOfUltima.MGameboard
{
    public class IronElementType : IElementType
    {
        private static readonly Random Random = new Random(121);
        private readonly string _name;
        private readonly string _imagePath;
        private readonly string _detailImagePath;
        private readonly string _elementInfo;
        private readonly bool _hasLevelEnable;
        private readonly bool _isRessources;
        private readonly ElementType.Type _elementType;
        private readonly ElementProductionBonus _elementProductionBonus;

        public IronElementType()
        {
            _elementType = ElementType.Type.RESSOURCE_IRON;
            _name = ElementType.GetTypeName(_elementType);
            _isRessources = true;
            _hasLevelEnable = false;
            var select = Random.Next(1, 5);
            _imagePath = "pack://application:,,,/Resources/resource_iron_" + select.ToString() + ".png";
            _detailImagePath = "pack://application:,,,/Resources/menu_iron.png";
            _elementInfo = "Each Iron Mine gains 50% for the first adjacent iron node, then 40% for each after that. The Iron Mine's production efficiency can then be increased up to 75% by an adjacent Foundry, and up to 30% by each adjacent Cottage.";

            // Bonus linked to iron
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
        public ElementType.Type GetElementType() { return _elementType; }
        public ElementCost GetElementCost(int level) { return null; } // Return null because there is no level with ressources
        public ElementProduction GetElementProduction(int level) { return null; }
        public ElementProductionBonus GetElementProductionBonus(int level) { return _elementProductionBonus; }

    }
}