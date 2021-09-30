using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SocialTechies_BlazorWebApp.Logger
{
    public static class CloudLogger
    {
        public static void logSlack(string message, string slackUrl)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(slackUrl))
                {
                    var content = JsonConvert.SerializeObject(new
                    {
                        text = message
                    });
                    DoWebRequest("POST", slackUrl, content, "application/json", "application/json, text/javascript, */*; q=0.01", out String response, out String error);
                }
            }
            catch { /* eat */ }
        }

        // dont want to use utilities here...
        public static bool DoWebRequest(String verb, String url, String postData, String contentType, String dataType, out String strResponse, out String error)
        {
            strResponse = "";
            error = "";

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.UserAgent = "AstuteSocial";
            req.ServicePoint.Expect100Continue = false;
            req.Method = verb;
            req.UseDefaultCredentials = true;

            if (dataType != null) req.Accept = dataType;
            if (contentType != null) req.ContentType = contentType;

            using (StreamWriter sw = new StreamWriter(req.GetRequestStream()))
            {
                sw.Write(postData);
                sw.Close();
            }

            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                {
                    if (resp != null)
                    {
                        using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                        {
                            strResponse = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                error = ex.Message;
                return false;
            }
            return true;
        }
    }
}
