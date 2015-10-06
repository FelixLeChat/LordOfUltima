using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LordOfUltima.Web
{
    class Chat
    {
        private static Chat _ins;

        /*
         * Fonction pour le patron Singleton
        */
        public static Chat Instance
        {
            get { return _ins ?? (_ins = new Chat()); }
        }

        private int _insertId;
        public List<string> GetLastChatString()
        {
            if (!IsInit)
                return null;

            string url = "http://api.felixlrc.ca/lou/chat.php?action=getchat&lastupdate=" + _insertId;

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
                        if (responseStream == null)
                            return null;

                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        String responseString = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(responseString))
                        {
                            string pattern = @"~PLAYERNAME~:([^~]*)~TEXT~:([^~]*)";

                            List<string> newLines = (from Match match in Regex.Matches(responseString, pattern) select match.Groups[2].Value).ToList();

                            pattern = @"~LASTID~:([0-9]*)$";
                            Match matchId = Regex.Match(responseString, pattern);
                            if(matchId.Success)
                            {
                                try
                                {
                                    _insertId = Convert.ToInt32(matchId.Groups[1].Value);
                                }
                                catch (Exception)
                                {
                                    _insertId = 0;
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
                    Console.WriteLine(resp.StatusCode == HttpStatusCode.NotFound ? "404 not found" : "Other Web Error 1");
                }
                else
                {
                    Console.WriteLine("Other Web Error 2 (No internet)");
                }
            }
            return null;
        }

        public bool IsInit;
        public void InitChat()
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
                        if (responseStream == null)
                            return;

                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        String responseString = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(responseString))
                        {
                            IsInit = true;
                            try
                            {
                                _insertId = Convert.ToInt32(responseString);
                            }
                            catch(Exception)
                            {
                                IsInit = false;
                                _insertId = 0;
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
                    Console.WriteLine(resp.StatusCode == HttpStatusCode.NotFound ? "404 not found" : "Other Web Error 1");
                }
                else
                {
                    Console.WriteLine("Other Web Error 2 (No internet)");
                }
            }
        }

        private static readonly Object _lockChat = new Object();
        public void InsertNewChatLine(object textobj)
        {
            string text = User.User.Instance.Name + " : " + (string)textobj;
            if (text.Length < 1)
                return;

            string url = "http://api.felixlrc.ca/lou/chat.php?action=chat_newline&text=" + Uri.EscapeDataString(text);
            try
            {
                // Construction requete
                var request = (HttpWebRequest)WebRequest.Create(url);

                lock(_lockChat)
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
                    Console.WriteLine(resp.StatusCode == HttpStatusCode.NotFound ? "404 not found" : "Other Web Error 1");
                }
                else
                {
                    Console.WriteLine("Other Web Error 2 (No internet)");
                }
            }
        }
    }
}
