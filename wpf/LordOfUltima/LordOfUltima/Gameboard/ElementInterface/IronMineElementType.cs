using System;

namespace LordOfUltima.MGameboard
{
    public class IronMineElementType : IElementType
    {
        private readonly string _name;
        private readonly string _imagePath;
        private readonly bool _hasLevelEnable;
        private readonly bool _isRessources;

        public IronMineElementType()
        {
            _name = ElementType.getTypeName(ElementType.type.BUILDING_IRONMINE);
            _isRessources = false;
            _hasLevelEnable = true;
            _imagePath = "Media/building/building_iron_quary.png";
        }

        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public bool IsRessources() { return _isRessources; }
        public string getImagePath() { return _imagePath; }
    }
}