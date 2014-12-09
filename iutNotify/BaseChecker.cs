using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Web.Script.Serialization;

namespace dotNetClientAgent
{
    abstract class BaseChecker
    {
        protected Thread t;
        protected volatile bool threadRunning = true;
        protected readonly object threadLock = new object();

        protected JavaScriptSerializer json;
        protected string login;
        protected Form1 f;

        public BaseChecker(Form1 form, string login)
        {
            this.f = form;
            this.login = login;
            json = new JavaScriptSerializer();
            t = new Thread(new ThreadStart(ThreadLoop));
            t.Start();
        }

        abstract protected void ThreadLoop();

        public void Stop()
        {
            threadRunning = false;
            lock (threadLock)
            {
                Monitor.Pulse(threadLock);
            }
        }
    }
}
