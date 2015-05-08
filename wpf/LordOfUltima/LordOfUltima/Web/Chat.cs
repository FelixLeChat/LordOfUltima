using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
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

        public int m_lastUpdate = 0;
        public string getLastChatString()
        {
            string url = "http://api.felixlrc.ca/lou/chat.php?action=getchat&lastupdate=";
            if (m_lastUpdate == 0)
            {
                url += "null";
            }
            else
            {
                url += m_lastUpdate;
            }
            string queryResult = "Felix : lorem ipsum sutre eu dura es.";
            return queryResult;
        }

        public void insertNewChatLine(string text)
        {
            string url = "http://api.felixlrc.ca/lou/chat.php?action=chat_newline&text=" + System.Uri.EscapeDataString(text);
            try
            {
                // Construction requete
                var request = (HttpWebRequest)WebRequest.Create(url);

                // Envoie de la requete et attente de la reponse
                using (var response = request.GetResponse())
                { }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        Console.WriteLine("404 not found");
                    }
                    else
                    {
                        Console.WriteLine("Other Web Error 1");
                    }
                }
                else
                {
                    Console.WriteLine("Other Web Error 2 (No internet)");
                }
            }
        }
    }
}
