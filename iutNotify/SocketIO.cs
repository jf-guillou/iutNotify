using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quobject.SocketIoClientDotNet.Client;

namespace iutNotify
{
    class SocketIO
    {
        public SocketIO()
        {
            var socket = IO.Socket("ws://intranet.iutsb.local:443/");
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                Console.WriteLine("Connected");

                socket.Emit("itsmemario", new string[] { "jeguill3" });
            });
            socket.On("quotaupdate", (o) =>
            {
                Console.WriteLine(o);
            });
            socket.On("messagesupdate", (o) =>
            {
                Console.WriteLine(o);
            });

            socket.On(Socket.EVENT_CONNECT_ERROR, (o) =>
            {
                Console.WriteLine("ConnErr");
                Console.WriteLine(o);
            });
            socket.On(Socket.EVENT_CONNECT_TIMEOUT, () => { Console.WriteLine("ConnTO"); });
            socket.On(Socket.EVENT_DISCONNECT, () => { Console.WriteLine("Disconn"); });
        }
    }
}
