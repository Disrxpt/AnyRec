using AnyRec.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnyRec.Server
{
    internal class Logging
    {
        private static readonly object ConsoleLock = new object();
        public static bool Enabled = false;
        public static bool ShowTimestamp = true;
        public static void Log(string server, string uri, string message)
        {
            Write("INFO", server, $"{uri} {message}", ConsoleColor.White);
        }
        public static void Info(string server, string message)
        {
            Write("INFO", server, message, ConsoleColor.White);
        }
        public static void Success(string server, string message)
        {
            Write("SUCCESS", server, message, ConsoleColor.Green);
        }
        public static void Warning(string server, string message)
        {
            Write("WARN", server, message, ConsoleColor.Yellow);
        }
        public static void Error(string server, string message)
        {
            Write("ERROR", server, message, ConsoleColor.Red);
        }
        public static void Debug(string server, string message)
        {
            if (!Config.GetBool("DeveloperMode")) return;
            Write("DEBUG", server, message, ConsoleColor.DarkGray);
        }
        public static void Request(string server, string method, string uri, int statusCode, long elapsedMs = 0)
        {
            ConsoleColor color = statusCode >= 500 ? ConsoleColor.Red : statusCode >= 400 ? ConsoleColor.Yellow : ConsoleColor.Green;
            Write(method.ToUpperInvariant(), server, $"{uri} → {statusCode} · {elapsedMs} ms", color);
        }
        public static void Exception(string server, Exception exception)
        {
            Write("ERROR", server, exception.ToString(), ConsoleColor.Red);
        }
        public static void Startup(string server, string address)
        {
            Write("START", server, $"Listening on {address}", ConsoleColor.Green);
        }
        public static void Shutdown(string server)
        {
            Write("STOP", server, "Server stopped.", ConsoleColor.Yellow);
        }
        private static void Write(string level, string server, string message, ConsoleColor color)
        {
            if (!Enabled) return;
            lock (ConsoleLock)
            {
                ConsoleColor original = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = color;
                    if (ShowTimestamp)
                    {
                        Console.Write($"[{DateTime.Now:HH:mm:ss}] ");
                    }
                    Console.Write($"[{level.PadRight(7)}] ");
                    Console.Write($"[{server}] ");
                    Console.WriteLine(message);
                }
                finally
                {
                    Console.ForegroundColor = original;
                }
            }
        }
    }
}
