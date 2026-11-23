using AnyRec.ServerBehaviors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AnyRec.Server
{
    internal class ServerStart
    {
        public static void StartServers(Process p, int buildid)
        {
            Console.Clear();
            Logging.Enabled = true;
            var socket = new SocketServer();
            if (33 <= buildid)
            {
                new Server(new APIServerBehavior());
            }
            p.WaitForExit();
            socket.Stop();
            foreach (var server in ServerBehavior.behaviorlist)
            {
                server.Stop();
            }
            ServerBehavior.behaviorlist = new List<Server>();
            Logging.Enabled = false;
            Program.Main(new string[0]);
        }
    }
}
