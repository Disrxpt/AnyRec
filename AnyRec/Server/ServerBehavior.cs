using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AnyRec.Server
{
    internal class ServerBehavior
    {
        public int Port { get; set; }
        public string Name { get; set; }
        public Func<HttpListenerContext, string, byte[]> Listener { get; set; }
        public static List<Server> behaviorlist = new List<Server>();
    }
}
