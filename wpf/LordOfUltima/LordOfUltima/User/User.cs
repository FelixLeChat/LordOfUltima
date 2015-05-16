using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordOfUltima.User
{
    class User
    {
        private static User m_ins = null;

        public static User getInstance()
        {
            if(m_ins == null)
            {
                m_ins = new User();
            }
            return m_ins;
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
