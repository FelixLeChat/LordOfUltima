using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LordOfUltima.Web
{
    class Utility
    {
        private static Utility m_ins = null;

        public static Utility getInstance()
        {
            if(m_ins == null)
            {
                m_ins = new Utility();
            }
            return m_ins;
        }

        public string getPlayerName(string email)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://api.felixlrc.ca/lou/request/lou.php");

            try
            {
                var postData = "action=getname";
                postData += "&email=" + email;
                postData += "&token=" + Login.getInstance().getToken();
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                // Envoie de la requete et attente de la reponse
                using (var response = request.GetResponse())
                {
                    // Obtenir le stream de la reponse
                    using (var responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        String responseString = reader.ReadToEnd();

                        if (responseString != "" && responseString.Length < 30 && Regex.IsMatch(responseString, @"^[a-zA-Z0-9]*$"))
                        {
                            return responseString;
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

            return "";
        }
    }
}
