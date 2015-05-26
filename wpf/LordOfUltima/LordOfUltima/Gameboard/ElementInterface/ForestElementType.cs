using System;

namespace LordOfUltima.MGameboard
{
    public class ForestElementType : IElementType
    {
        private static readonly Random Random = new Random(12221);
        private readonly string _name;
        private readonly string _imagePath;
        private readonly bool _hasLevelEnable;
        private readonly bool _isRessources;
        private readonly ElementType.type _elementType;

        public ForestElementType()
        {
            _elementType = ElementType.type.RESSOURCE_FOREST;
            _name = ElementType.getTypeName(_elementType);
            _isRessources = true;
            _hasLevelEnable = false;
            var select = Random.Next(1, 5);
            _imagePath = "Media/ressource/resource_wood_" + select.ToString() + ".png";
        }

        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public bool IsRessources() { return _isRessources; }
        public string getImagePath() { return _imagePath; }
        public ElementType.type GetElementType() { return _elementType;}
        public ElementCost GetElementCost(int level) { return null;} // Return null because there is no level with ressources
    }
}
