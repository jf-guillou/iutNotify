using System;
using System.Collections.Generic;

namespace iutNotify
{
    class Ping
    {
        public string status { get; set; }
        public PrintQuota pquota { get; set; }
        public DiskQuota dquota { get; set; }
        public List<NetworkMessage> messages { get; set; }
    }

    class PrintQuota
    {
        public int quota { get; set; }
        public int maxQuota { get; set; }
        public bool hasQuota { get; set; }
    }

    class DiskQuota
    {
        public int quota { get; set; }
        public int maxQuota { get; set; }
        public bool hasQuota { get; set; }
    }

    public class NetworkMessage
    {
        public int id { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string addedAt { get; set; }
        public bool global { get; set; }
        public bool notif { get; set; }

        public DateTime dateAdded { get { return DateTime.Parse(addedAt); } }
    }
}
