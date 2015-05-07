using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordOfUltima.Web
{
    class Chat
    {
        private static Chat m_ins = null;

        /*
         * Fonction pour le patron Singleton
        */
        public static Chat getInstance()
        {
            if(m_ins == null)
            {
                m_ins = new Chat();
            }
            return m_ins;
        }

        public string getLastChatString(DateTime dateTime)
        {
            string queryResult = "Felix : lorem ipsum sutre eu dura es.";
            return queryResult;
        }
    }
}
