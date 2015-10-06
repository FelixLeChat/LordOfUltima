namespace LordOfUltima.Error
{
    class Error
    {
        public Type Description { get; set; }

        // Total time that the error will be displayed
        private int _timeTotal = 5;
        public int TimeTotal
        {
            get { return _timeTotal; }
            set { _timeTotal = value; }
        }

        // Time remaining on the display of the error
        private int _timeRemaining = 5;
        public int TimeRemaining
        {
            get { return _timeRemaining; }
            set { _timeRemaining = value; }
        }

        public int RespawnCount { get; set; }

        public enum Type
        {
            // Ressources
            NOT_ENOUGH_RESSOURCES_BUILD,
            NOT_ENOUGH_RESSOURCES_UPGRADE,
            NOT_ENOUGH_RESSOURCES_RESEARCH,
            NOT_ENOUGH_BUILDING_SPACE,
            WOOD_STORAGE_FULL,
            STONE_STORAGE_FULL,
            IRON_STORAGE_FULL,
            FOOD_STORAGE_FULL,

            // Save
            SAVED_GAME_NOT_FOUND,

            // Time option
            INVALID_TIME,
        }

        public string GetDescriptionString()
        {
            var result = "";
            switch (Description)
            {
                case Type.NOT_ENOUGH_RESSOURCES_BUILD:
                    result = "Not enough ressources to build";
                    break;
                case Type.NOT_ENOUGH_RESSOURCES_UPGRADE:
                    result = "Not enough ressources to upgrade";
                    break;
                case Type.NOT_ENOUGH_RESSOURCES_RESEARCH:
                    result = "Not enough ressources to research";
                    break;
                case Type.NOT_ENOUGH_BUILDING_SPACE:
                    result = "Missing building space";
                    break;
                case Type.SAVED_GAME_NOT_FOUND:
                    result = "Saved Game not found";
                    break;
                case Type.INVALID_TIME:
                    result = "Invalid time entered";
                    break;
                case Type.WOOD_STORAGE_FULL:
                    result = "Wood Storage is full";
                    break;
                case Type.STONE_STORAGE_FULL:
                    result = "Stone Storage is full";
                    break;
                case Type.IRON_STORAGE_FULL:
                    result = "Iron Storage is full";
                    break;
                case Type.FOOD_STORAGE_FULL:
                    result = "Food Storage is full";
                    break;
            }
            return result;
        }
    }
}
