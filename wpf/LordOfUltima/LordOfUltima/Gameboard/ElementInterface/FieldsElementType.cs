using System;

namespace LordOfUltima.MGameboard
{
    public class FieldsElementType : IElementType
    {
        private readonly string _name;
        private readonly string _imagePath;
        private readonly string _detailImagePath;
        private readonly string _elementInfo;
        private readonly bool _hasLevelEnable;
        private readonly bool _isRessources;
        private readonly ElementType.type _elementType;

        public FieldsElementType()
        {
            _elementType = ElementType.type.RESSOURCE_FIELDS;
            _name = ElementType.getTypeName(_elementType);
            _isRessources = true;
            _hasLevelEnable = false;
            _imagePath = "Media/ressource/resource_corn.png";
            _detailImagePath = "Media/menu/menu_forest.png";
            _elementInfo = "Each Woodcutter's Hut gains 50% production efficiency from the first adjacent wood node, then 40% for each after that. The Woodcutter's Hut production efficiency can then be further increased up to 75% by an adjacent Sawmill, and up to 30% by each adjacent Cottage.";
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
