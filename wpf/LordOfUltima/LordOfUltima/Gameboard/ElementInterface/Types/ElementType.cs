namespace LordOfUltima.MGameboard
{
    public class ElementType
    {
        public enum Type
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
            BUILDING_TOWNHALL,
            BUILDING_TOWNHOUSE,
            BUILDING_MARKETPLACE,
            BUILDING_RESEARCH_CENTER,
            BUILDING_WAREHOUSE
        }

        public static string GetTypeName(Type resstype)
        {
            string name = "";
            switch(resstype)
            {
                case Type.RESSOURCE_FOREST:
                    name = "Forest";
                    break;
                case Type.RESSOURCE_STONE:
                    name = "Stone";
                    break;
                case Type.RESSOURCE_IRON:
                    name = "Iron";
                    break;
                case Type.RESSOURCE_WATER:
                    name = "Water";
                    break;
                case Type.RESSOURCE_FIELDS:
                    name = "Field";
                    break;
                case Type.BUILDING_FARM:
                    name = "Farm";
                    break;
                case Type.BUILDING_WOODCUTTER:
                    name = "Woodcutter's Hut";
                    break;
                case Type.BUILDING_SAWMILL:
                    name = "Sawmill";
                    break;
                case Type.BUILDING_QUARRY:
                    name = "Quarry";
                    break;
                case Type.BUILDING_STONEMASON:
                    name = "Stone Mason";
                    break;
                case Type.BUILDING_IRONMINE:
                    name = "Iron Mine";
                    break;
                case Type.BUILDING_FOUNDRY:
                    name = "Foundry";
                    break;
                case Type.BUILDING_TOWNHALL:
                    name = "Town Hall";
                    break;
                case Type.BUILDING_MILL:
                    name = "Mill";
                    break;
                case Type.BUILDING_TOWNHOUSE:
                    name = "Townhouse";
                    break;
                case Type.BUILDING_MARKETPLACE:
                    name = "Marketplace";
                    break;
                case Type.BUILDING_RESEARCH_CENTER:
                    name = "Research Center";
                    break;
                case Type.BUILDING_WAREHOUSE:
                    name = "Warehouse";
                    break;
            }
            return name;
        }

        public static IElementType GetTypeObject(Type ressType)
        {
            IElementType result = null;
            switch (ressType)
            {
                case Type.RESSOURCE_FOREST:
                    result = new ForestElementType();
                    break;
                case Type.RESSOURCE_STONE:
                    result = new StoneElementType();
                    break;
                case Type.RESSOURCE_IRON:
                    result = new IronElementType();
                    break;
                case Type.RESSOURCE_WATER:
                    result = new LakeElementType();
                    break;
                case Type.RESSOURCE_FIELDS:
                    result = new FieldsElementType();
                    break;
            }
            return result;
        }

        public static Type GetBonusRessource(Type buildingType)
        {
            Type result = Type.DEFAULT;

            switch (buildingType)
            {
                case Type.BUILDING_WOODCUTTER:
                    result = Type.RESSOURCE_FOREST;
                    break;
                case Type.BUILDING_QUARRY:
                    result = Type.RESSOURCE_STONE;
                    break;
                case Type.BUILDING_IRONMINE:
                    result = Type.RESSOURCE_IRON;
                    break;
                case Type.BUILDING_FARM:
                    result = Type.RESSOURCE_WATER;
                    break;
            }
            return result;
        }

        public static IElementType GetElementFromType(string elementType)
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
                case "RESSOURCE_FIELDS":
                    newElement = new FieldsElementType();
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
                case "BUILDING_TOWNHOUSE":
                    newElement = new TownhouseElementType();
                    break;
                case "BUILDING_MARKETPLACE":
                    newElement = new MarketplaceElementType();
                    break;
                case "BUILDING_RESEARCH_CENTER":
                    newElement = new ResearchCenterElementType();
                    break;
                case "BUILDING_WAREHOUSE":
                    newElement = new WarehouseElementType();
                    break;
            }

            return newElement;
        }

        public static Type GetBonusBuilding(Type buildingType)
        {
            Type elementType = Type.DEFAULT;

            switch (buildingType)
            {
                case Type.BUILDING_WOODCUTTER:
                    elementType = Type.BUILDING_SAWMILL;
                    break;
                case Type.BUILDING_QUARRY:
                    elementType = Type.BUILDING_STONEMASON;
                    break;
                case Type.BUILDING_IRONMINE:
                    elementType = Type.BUILDING_FOUNDRY;
                    break;
                case Type.BUILDING_FARM:
                    elementType = Type.BUILDING_MILL;
                    break;
                case Type.BUILDING_TOWNHOUSE:
                    elementType = Type.BUILDING_MARKETPLACE;
                    break;
            }
            return elementType;
        }
    }
}
