using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Threading;

namespace iutNotify
{
    class DataPush
    {
        private static int LOOP_SEC = 30;
        //private static string API_VERSION = "1";
        private static string BASE_URL = "http://intranet.iutsb.local/api/ping.php";

        protected Thread t;
        protected volatile bool threadRunning = true;
        protected readonly object threadLock = new object();

        protected MainForm f;

        private Socket socket;
        private string login;

        public delegate void WebSocketStarter(DataPush p, WebsocketStarterEventArgs e);
        public event WebSocketStarter WSStart;

        public DataPush(MainForm form, string login)
        {
            this.f = form;
            this.login = login;

            this.WSStart += OnWebsocketStart;

            t = new Thread(new ThreadStart(ThreadLoop));
            t.Name = "GetWSAddress";
            t.Start();
        }

        protected void ThreadLoop()
        {
            while (threadRunning)
            {
                string rawdata = HttpWrapper.Get(BASE_URL + "?websocketbind");
                
                if (rawdata.IndexOf("ws://") == 0)
                {
                    if (WSStart != null)
                    {
                        WSStart(this, new WebsocketStarterEventArgs()
                        {
                            address = rawdata
                        });
                        threadRunning = false;
                        break;
                    }
                }
                lock (threadLock) { Monitor.Wait(threadLock, TimeSpan.FromSeconds(LOOP_SEC)); }
            }
        }

        protected void OnWebsocketStart(object sender, WebsocketStarterEventArgs e)
        {
            socket = IO.Socket(e.address);
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                //Console.WriteLine("Connected");

                socket.Emit("itsmemario", new string[] { login });
            });
            socket.On("quotaupdate", (o) =>
            {
                JObject j = (JObject)o;

                if (j != null)
                {
                    if ((int)j.SelectToken("has_quota") == 1)
                        f.SetPrintQuota((int)j.SelectToken("quota"), (int)j.SelectToken("max_quota"));
                    else
                        f.SetPrintQuota(false);
                }
                else
                    f.SetPrintQuota();
                f.FrameFilled();
            });
            socket.On("messagesupdate", (o) =>
            {
                JArray jArr = (JArray)o;

                foreach(JToken j in jArr) {
                    f.EnqueueBubble((string)j.SelectToken("title"), 
                        (string)j.SelectToken("message"), 
                        (DateTime)j.SelectToken("added_at"), 
                        (string)j.SelectToken("url"), 
                        (int)j.SelectToken("notif") == 1
                    );
                }
            });
            /*
            socket.On(Socket.EVENT_CONNECT_ERROR, (o) => { Console.WriteLine("ConnErr"); });
            socket.On(Socket.EVENT_CONNECT_TIMEOUT, () => { Console.WriteLine("ConnTO"); });
            socket.On(Socket.EVENT_DISCONNECT, () => { Console.WriteLine("Disconn"); });
            */
        }

        public void RequestStop()
        {
            if (socket != null)
            {
                socket.Disconnect();
            }

            if (threadRunning)
            {
                threadRunning = false;
                lock (threadLock)
                {
                    Monitor.Pulse(threadLock);
                }
            }
        }
    }

    class WebsocketStarterEventArgs : EventArgs
    {
        public string address { get; set; }
    }
}
