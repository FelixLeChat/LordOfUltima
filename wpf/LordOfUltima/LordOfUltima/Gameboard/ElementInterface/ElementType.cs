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
            RESSOURCE_FIELDS
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
