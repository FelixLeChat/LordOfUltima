using System;

namespace LordOfUltima.MGameboard
{
    public class LakeElementType : IElementType
    {
        private readonly string _name;
        private readonly string _imagePath;
        private readonly bool _hasLevelEnable;
        private readonly bool _isRessources;

        public LakeElementType()
        {
            _name = ElementType.getTypeName(ElementType.type.RESSOURCE_WATER);
            _isRessources = true;
            _hasLevelEnable = false;
            _imagePath = "Media/ressource/resource_lake.png";
        }

        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public bool IsRessources() { return _isRessources; }
        public string getImagePath() { return _imagePath; }
    }
}