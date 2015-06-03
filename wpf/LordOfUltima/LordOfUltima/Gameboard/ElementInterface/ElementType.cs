namespace LordOfUltima.MGameboard
{
    public class ElementType
    {
        public enum type
        {
            DEFAULT,
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
            BUILDING_FOUNDRY,
            BUILDING_FARM,
            BUILDING_MILL,

            BUILDING_TOWNHALL
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
                case type.BUILDING_FARM:
                    name = "Farm";
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
                case type.BUILDING_TOWNHALL:
                    name = "Town Hall";
                    break;
                case type.BUILDING_MILL:
                    name = "Mill";
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
                case type.RESSOURCE_WATER:
                    result = new LakeElementType();
                    break;
                /*case type.RESSOURCE_FIELDS:
                    name = "Field";
                    break;*/
                default:
                    break;
            }
            return result;
        }

        public static type getBonusRessource(type buildingType)
        {
            type result = type.DEFAULT;

            switch (buildingType)
            {
                case type.BUILDING_WOODCUTTER:
                    result = type.RESSOURCE_FOREST;
                    break;
                case type.BUILDING_QUARRY:
                    result = type.RESSOURCE_STONE;
                    break;
                case type.BUILDING_IRONMINE:
                    result = type.RESSOURCE_IRON;
                    break;
                case type.BUILDING_FARM:
                    result = type.RESSOURCE_WATER;
                    break;
                default:
                    break;
            }
            return result;
        }

        public static IElementType getElementFromType(string elementType)
        {
            IElementType newElement = null;
            switch(elementType)
            {
                case "RESSOURCE_FOREST":
                    newElement = new ForestElementType();
                    break;
                case "RESSOURCE_STONE":
                    newElement = new StoneElementType();
                    break;
                case "RESSOURCE_IRON":
                    newElement = new IronElementType();
                    break;
                case "RESSOURCE_WATER":
                    newElement = new LakeElementType();
                    break;

                case "BUILDING_WOODCUTTER":
                    newElement = new WoodcutterElementType();
                    break;
                case "BUILDING_SAWMILL":
                    newElement = new SawmillElementType();
                    break;
                case "BUILDING_QUARRY":
                    newElement = new QuarryElementType();
                    break;
                case "BUILDING_STONEMASON":
                    newElement = new StoneMasonElementType();
                    break;
                case "BUILDING_IRONMINE":
                    newElement = new IronMineElementType();
                    break;
                case "BUILDING_FOUNDRY":
                    newElement = new FoundryElementType();
                    break;
                case "BUILDING_TOWNHALL":
                    newElement = new TownHallElementType();
                    break;
                case "BUILDING_FARM":
                    newElement = new FarmElementType();
                    break;
                case "BUILDING_MILL":
                    newElement = new MillElementType();
                    break;
            }

            return newElement;
        }

    }
}
