using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

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

        private int m_insert_id = 0;
        public List<string> getLastChatString()
        {
            if (!is_init)
                return null;

            string url = "http://api.felixlrc.ca/lou/chat.php?action=getchat&lastupdate=" + m_insert_id;

            try
            {
                // Construction requete
                var request = (HttpWebRequest)WebRequest.Create(url);

                // Envoie de la requete et attente de la reponse
                using (var response = request.GetResponse())
                {
                    // Obtenir le stream de la reponse
                    using (var responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        String responseString = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(responseString))
                        {
                            List<string> newLines = new List<string>();
                            string pattern = @"~PLAYERNAME~:([^~]*)~TEXT~:([^~]*)";

                            foreach (Match match in Regex.Matches(responseString, pattern))
                            {
                                newLines.Add(match.Groups[2].Value);
                            }

                            pattern = @"~LASTID~:([0-9]*)$";
                            Match matchId = Regex.Match(responseString, pattern);
                            if(matchId.Success)
                            {
                                try
                                {
                                    m_insert_id = Convert.ToInt32(matchId.Groups[1].Value);
                                }
                                catch (Exception e)
                                {
                                    m_insert_id = 0;
                                }
                            }
                            return newLines;
                        }
                        return null;
                    }
                }
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
            return null;
        }

        public bool is_init = false;
        public void initChat()
        {
            string url = "http://api.felixlrc.ca/lou/chat.php?action=init";

            try
            {
                // Construction requete
                var request = (HttpWebRequest)WebRequest.Create(url);

                // Envoie de la requete et attente de la reponse
                using (var response = request.GetResponse())
                {
                    // Obtenir le stream de la reponse
                    using (var responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        String responseString = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(responseString))
                        {
                            is_init = true;
                            try
                            {
                                m_insert_id = Convert.ToInt32(responseString);
                            }
                            catch(Exception e)
                            {
                                is_init = false;
                                m_insert_id = 0;
                            }

                        }
                    }
                }
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

        private static Object m_lock_chat = new Object();
        public void insertNewChatLine(object textobj)
        {
            string text = User.User.getInstance().Name + " : " + (string)textobj;
            if (text.Length < 1)
                return;

            string url = "http://api.felixlrc.ca/lou/chat.php?action=chat_newline&text=" + System.Uri.EscapeDataString(text);
            try
            {
                // Construction requete
                var request = (HttpWebRequest)WebRequest.Create(url);

                lock(m_lock_chat)
                {
                    // Envoie de la requete et attente de la reponse
                    using (var response = request.GetResponse())
                    { }
                }

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
