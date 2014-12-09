using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace dotNetClientAgent
{
    class DiskQuotaChecker : BaseChecker
    {
        private static int LOOP_MIN = 10;
        private static int API_VERSION = 1;
        private static string BASE_URL = "http://intranet.iutsb.local/wspq/FILLME";
        
        public DiskQuotaChecker(Form1 form, string login)
            : base(form, login)
        {
            t.Name = "DiskQuotaChecker";
        }

        protected override void ThreadLoop()
        {
            while (threadRunning)
            {
                string url = String.Format("{0}?login={1}&v=", BASE_URL, login, API_VERSION);
                string rawData = HttpWrapper.Get(url);
                DiskQuota d = json.Deserialize<DiskQuota>(rawData);

                if (d != null)
                {
                    
                }

                lock (threadLock)
                {
                    Monitor.Wait(threadLock, TimeSpan.FromMinutes(LOOP_MIN));
                }
            }
        }
    }

    class DiskQuota
    {
    }
}
