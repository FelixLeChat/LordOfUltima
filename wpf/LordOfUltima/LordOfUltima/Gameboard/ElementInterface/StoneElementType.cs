using System;

namespace LordOfUltima.MGameboard
{
    public class StoneElementType : IElementType
    {
        private readonly string _name;
        private readonly string _imagePath;
        private readonly string _detailImagePath;
        private readonly string _elementInfo;
        private readonly bool _hasLevelEnable;
        private readonly bool _isRessources;
        private readonly ElementType.type _elementType;

        public StoneElementType()
        {
            _elementType = ElementType.type.RESSOURCE_STONE;
            _name = ElementType.getTypeName(_elementType);
            _isRessources = true;
            _hasLevelEnable = false;
            _imagePath = "Media/ressource/resource_stone.png";
            _detailImagePath = "Media/menu/menu_stone.png";
            _elementInfo = "Each Quarry gains 50% for the first adjacent stone node, then 40% for each after that. The Quarry's production efficiency can then be increased up to 75% by an adjacent Stonemason, and up to 30% by each adjacent Cottage.";
        }

        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public bool IsRessources() { return _isRessources; }
        public string getImagePath() { return _imagePath; }
        public string getDetailImagePath() { return _detailImagePath; }
        public string GetElementInfo() { return _elementInfo; }
        public ElementType.type GetElementType() { return _elementType; }
        public ElementCost GetElementCost(int level) { return null; } // Return null because there is no level with ressources

    }
}