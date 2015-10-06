namespace LordOfUltima.User
{
    class User
    {
        private static User _user;
        public static User Instance
        {
            get { return _user ?? (_user = new User()); }
        }

        private string _name = "";
        private string _email = "";

        // methods to set or get player name
        public string Name
        {
            get 
            { 
                return _name; 
            }
            set
            {
                // Can only set the name once
                if(_name == "")
                {
                    _name = value;
                }
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if( _email == "")
                {
                    _email = value;
                }
            }
        }
    }
}
