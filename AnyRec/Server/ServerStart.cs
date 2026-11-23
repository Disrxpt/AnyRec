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
            var socket = new SocketServer();
            if (33 <= buildid)
            {
                new Server(new APIServerBehavior());
            }
            p.WaitForExit();
            foreach (var server in ServerBehavior.behaviorlist)
            {
                server.Stop();
                socket.Stop();
            }
            Program.Main(new string[0]);
        }
    }
}
