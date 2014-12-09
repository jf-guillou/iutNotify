using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace dotNetClientAgent
{
    class PrintQuotaChecker : BaseChecker
    {
        private static int LOOP_MIN = 1;
        private static int API_VERSION = 1;
        private static string BASE_URL = "http://intranet.iutsb.local/wspq/quota.php";

        private PrintQuota pQuota;

        public PrintQuotaChecker(Form1 form, string login)
            : base(form, login)
        {
            t.Name = "PrintQuotaChecker";
        }

        protected override void ThreadLoop()
        {
            while (threadRunning)
            {
                string url = String.Format("{0}?login={1}&v=", BASE_URL, login, API_VERSION);
                string rawData = HttpWrapper.Get(url);
                PrintQuota p = json.Deserialize<PrintQuota>(rawData);

                if (p != null)
                {
                    if (pQuota == null || pQuota != p)
                    {
                        pQuota = p;
                        if (p.has_quota)
                            f.SetPrintQuota(p.quota + " page" + (p.quota != 1 ? "s" : ""));
                        else
                            f.SetPrintQuota("illimité");
                    }
                }

                lock (threadLock)
                {
                    Monitor.Wait(threadLock, TimeSpan.FromMinutes(LOOP_MIN));
                }
            }
        }
    }

    class PrintQuota
    {
        public bool has_quota { get; set; }
        public int quota { get; set; }
    }
}
