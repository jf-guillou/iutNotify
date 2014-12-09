using System;
using System.Linq;
using System.Threading;
using System.Web.Script.Serialization;
using Utils.Web;

namespace iutNotify
{
    class DataPush
    {
        private static int LOOP_SEC = 30;
        private static string API_VERSION = "1";
        private static string BASE_URL = "http://intranet.iutsb.local/api/ping.php";

        protected Thread t;
        protected volatile bool threadRunning = true;
        protected readonly object threadLock = new object();

        protected JavaScriptSerializer json;
        protected MainForm f;

        private PrintQuota pQuota;

        private volatile QueryString query;
        
        public DataPush(MainForm form, string login)
        {
            this.f = form;
            query = new QueryString().Add("login", login).Add("v", API_VERSION);
            
            t = new Thread(new ThreadStart(ThreadLoop));
            t.Name = "Pinger";
            t.Start();
        }

        protected void ThreadLoop()
        {
            json = new JavaScriptSerializer();
            query.Add("printquota", "1").Add("messages", "1");//.Add("diskquota", "1");

            while (threadRunning)
            {
                string url = BASE_URL + query.ToString();
                string rawData = HttpWrapper.Get(url);
                Ping ping = null;

                if (rawData != null && rawData != "")
                {
                    try { ping = json.Deserialize<Ping>(rawData); }
                    catch (Exception) { }
                }

                if (ping != null && f.Exists())
                {
                    if (ping.pquota != null)
                    {
                        if (pQuota == null || pQuota != ping.pquota)
                        {
                            pQuota = ping.pquota;
                            if (pQuota.hasQuota)
                                f.SetPrintQuota(pQuota.quota, pQuota.maxQuota);
                            else
                            {
                                f.SetPrintQuota(false);
                                query.Remove("printquota");
                            }
                        }
                    }
                    else if(query.Contains("printquota"))
                        f.SetPrintQuota();

                    /*
                    if (ping.dquota != null)
                    {
                    }
                    else if(query.Contains("diskquota"))
                        f.SetDiskQuota();
                    */
                    f.FrameFilled();

                    if (ping.messages != null)
                    {
                        if(ping.messages.Count > 0) {
                            query.Add("messages", ping.messages.Last().id.ToString(), true);
                            f.AddBubbles(ping.messages);
                        }
                    }
                }

                lock (threadLock) { Monitor.Wait(threadLock, TimeSpan.FromSeconds(LOOP_SEC)); }
            }
        }

        public void RequestStop()
        {
            threadRunning = false;
            lock (threadLock)
            {
                Monitor.Pulse(threadLock);
            }
        }
    }
}
