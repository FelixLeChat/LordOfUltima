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

        public ForestElementType()
        {
            _name = ElementType.getTypeName(ElementType.type.RESSOURCE_FOREST);
            _isRessources = true;
            _hasLevelEnable = false;
            var select = Random.Next(1, 5);
            _imagePath = "Media/ressource/resource_wood_" + select.ToString() + ".png";
        }

        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public bool IsRessources() { return _isRessources; }
        public string getImagePath() { return _imagePath; }
    }
}
