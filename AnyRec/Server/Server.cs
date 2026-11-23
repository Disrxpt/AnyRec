using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace AnyRec.Server
{
    internal class Server
    {
        public ServerBehavior behavior;
        public HttpListener listener;
        public bool listening;
        public Server(ServerBehavior behavior)
        {
            this.behavior = behavior;
            ServerBehavior.behaviorlist.Add(this);
            try
            {
                Logging.Info(behavior.Name, "Starting server...");
                _ = this.Listener();
            }
            catch
            {
                Logging.Error(behavior.Name, "Failed to start server.");
            }
        }
        public async Task Listener()
        {
            listener = new HttpListener();
            listener.Prefixes.Add($"https://localhost:{behavior.Port}/");
            listener.Start();
            listening = true;
            Logging.Success(behavior.Name, "Server started successfully!");
            while (listening)
            {
                var ctx = await listener.GetContextAsync();
                if (listening)
                {
                    string p = "";
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ctx.Request.InputStream.CopyTo(memoryStream);
                        byte[] p_array = memoryStream.ToArray();
                        p = Encoding.ASCII.GetString(p_array);
                    }
                    ctx.Response.Headers["Last-Modified"] = DateTime.UtcNow.ToString("R");
                    Logging.Info(this.behavior.Name, $"URI was requested [{ctx.Request.RawUrl}]");
                    if (!string.IsNullOrEmpty(p))
                    {
                        Logging.Debug(this.behavior.Name, $"PostData for {ctx.Request.Url.AbsolutePath}: {p}");
                    }
                    if (listening)
                    {
                        byte[] array = this.behavior.Listener(ctx);
                        ctx.Response.StatusCode = 200;
                        ctx.Response.ContentLength64 = array.Length;
                        await ctx.Response.OutputStream.WriteAsync(array);
                        ctx.Response.Close();
                    }
                }
            }
        }
        public void Stop()
        {
            listening = false;
            this.listener.Close();
        }
    }
}
