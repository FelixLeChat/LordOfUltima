using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordOfUltima.MGameboard
{
    class ElementType
    {
        public enum type
        {
            RESSOURCE_FOREST,
            RESSOURCE_STONE,
            RESSOURCE_IRON,
            RESSOURCE_WATER,
            RESSOURCE_FIELDS,

            BUILDING_WOODCUTTER,
            BUILDING_SAWMILL,
            BUILDING_QUARRY,
            BUILDING_STONEMASON,
            BUILDING_IRONMINE,
            BUILDING_FOUNDRY
        }

        public static string getTypeName(type resstype)
        {
            string name = "";
            switch(resstype)
            {
                case type.RESSOURCE_FOREST:
                    name = "Forest";
                    break;
                case type.RESSOURCE_STONE:
                    name = "Stone";
                    break;
                case type.RESSOURCE_IRON:
                    name = "Iron";
                    break;
                case type.RESSOURCE_WATER:
                    name = "Water";
                    break;
                case type.RESSOURCE_FIELDS:
                    name = "Field";
                    break;
                case type.BUILDING_WOODCUTTER:
                    name = "Woodcutter's Hut";
                    break;
                case type.BUILDING_SAWMILL:
                    name = "Sawmill";
                    break;
                case type.BUILDING_QUARRY:
                    name = "Quarry";
                    break;
                case type.BUILDING_STONEMASON:
                    name = "Stone Mason";
                    break;
                case type.BUILDING_IRONMINE:
                    name = "Iron Mine";
                    break;
                case type.BUILDING_FOUNDRY:
                    name = "Foundry";
                    break;
                default:
                    break;
            }
            return name;
        }

        public static IElementType getTypeObject(type ressType)
        {
            IElementType result = null;
            switch (ressType)
            {
                case type.RESSOURCE_FOREST:
                    result = new ForestElementType();
                    break;
                case type.RESSOURCE_STONE:
                    result = new StoneElementType();
                    break;
                case type.RESSOURCE_IRON:
                    result = new IronElementType();
                    break;
                /*case type.RESSOURCE_WATER:
                    name = "Water";
                    break;
                case type.RESSOURCE_FIELDS:
                    name = "Field";
                    break;*/
                default:
                    break;
            }
            return result;
        }
    }
}
