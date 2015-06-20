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

        public enum Type
        {
            NOT_ENOUGH_RESSOURCES_BUILD,
            NOT_ENOUGH_RESSOURCES_UPGRADE,
            NOT_ENOUGH_BUILDING_SPACE,
            SAVED_GAME_NOT_FOUND,

            INVALID_TIME
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
                case Type.NOT_ENOUGH_BUILDING_SPACE:
                    result = "Missing building space";
                    break;
                case Type.SAVED_GAME_NOT_FOUND:
                    result = "Saved Game not found";
                    break;
                case Type.INVALID_TIME:
                    result = "Invalid time entered";
                    break;
            }
            return result;
        }
    }
}
