using System;
using System.Threading;
using System.Windows.Forms;

namespace iutNotify
{
    static class Program
    {
        public static string MY_NAME = "iutNotify";
        public static string MY_VER = "0.1";
        static Mutex mutex = new Mutex(true, System.Diagnostics.Debugger.IsAttached ? "{94E104A0-6FC6-11E4-9803-0800200C9A65}" : "{94E104A0-6FC6-11E4-9803-0800200C9A66}");

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
                mutex.ReleaseMutex();
            }
        }
    }
}
