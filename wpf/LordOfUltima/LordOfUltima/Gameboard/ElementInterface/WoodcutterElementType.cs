﻿using System;

namespace LordOfUltima.MGameboard
{
    public class WoodcutterElementType : IElementType
    {
        private readonly string _name;
        private readonly string _imagePath;
        private readonly bool _hasLevelEnable;
        private readonly bool _isRessources;

        public WoodcutterElementType()
        {
            _name = ElementType.getTypeName(ElementType.type.BUILDING_WOODCUTTER);
            _isRessources = false;
            _hasLevelEnable = true;
            _imagePath = "Media/building/building_woodcutter.png";
        }

        public string Name() { return _name; }
        public bool HasLevelEnable() { return _hasLevelEnable; }
        public bool IsRessources() { return _isRessources; }
        public string getImagePath() { return _imagePath; }
    }
}