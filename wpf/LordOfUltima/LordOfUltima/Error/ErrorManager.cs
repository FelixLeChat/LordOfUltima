namespace LordOfUltima.Error
{
    class ErrorManager
    {
        private static ErrorManager _instance;
        public static ErrorManager Instance
        {
            get { return _instance ?? (_instance = new ErrorManager()); }
        }

        // TODO : set list of error and dispatch them

    }
}
