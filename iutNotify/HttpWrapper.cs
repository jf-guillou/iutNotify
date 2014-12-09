using System;
using System.IO;
using System.Net;

namespace iutNotify
{
    class HttpWrapper
    {
        public static string Get(string url)
        {
            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(url);
            wReq.UserAgent = Program.MY_NAME + "/" + Program.MY_VER;
            WebResponse wRes;
            try
            {
                wRes = wReq.GetResponse();
            }
            catch (Exception)
            {
                return "";
            }

            Stream dStr = wRes.GetResponseStream();
            StreamReader reader = new StreamReader(dStr);
            string res = reader.ReadToEnd();
            reader.Close();
            dStr.Close();
            wRes.Close();
            return res;
        }
    }
}
