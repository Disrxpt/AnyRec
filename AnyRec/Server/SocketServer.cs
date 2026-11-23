using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Net.WebSockets;
using System.Text;

namespace AnyRec.Server
{
    internal class SocketServer
    {
        public HttpListener listener;
        public bool listening;
        public SocketServer()
        {
            try
            {
                Logging.Info("NOTIF", "Starting socket server...");
                _ = this.Listener();
            }
            catch
            {
                Logging.Error("NOTIF", "Failed to start socket server.");
            }
        }
        public async Task Listener()
        {
            listener = new HttpListener();
            listener.Prefixes.Add($"https://localhost:{3001}/");
            listener.Start();
            listening = true;
            Logging.Success("NOTIF", "Socket server started successfully!");
            while (listening)
            {
                var ctx = await listener.GetContextAsync();
                if (!ctx.Request.IsWebSocketRequest)
                {
                    string p = "";
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        ctx.Request.InputStream.CopyTo(memoryStream);
                        byte[] p_array = memoryStream.ToArray();
                        p = Encoding.ASCII.GetString(p_array);
                    }
                    Logging.Info("NOTIF", $"URI was requested [{ctx.Request.RawUrl}]");
                    if (string.IsNullOrEmpty(p))
                    {
                        Logging.Debug("NOTIF", $"PostData for {ctx.Request.Url.AbsoluteUri}: {p}");
                    }
                    if (listening)
                    {
                        byte[] array = Bytes.StringToByte("200 OK");
                        ctx.Response.StatusCode = 200;
                        ctx.Response.ContentLength64 = array.Length;
                        await ctx.Response.OutputStream.WriteAsync(array);
                        ctx.Response.Close();
                    }
                }
                else
                {
                    HttpListenerWebSocketContext webSocketContext = await ctx.AcceptWebSocketAsync(null);
                    WebSocket webSocket = webSocketContext.WebSocket;
                    Logging.Success("NOTIF", "Connected to the websocket server!");
                    string clientId = Guid.NewGuid().ToString();
                    string requestUrl = ctx.Request.Url.AbsolutePath;
                    ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                    WebSocketReceiveResult result = null;
                    try
                    {
                        while (webSocket.State == WebSocketState.Open)
                        {
                            if (!listening)
                            {
                                webSocket.Abort();
                                webSocket.Dispose();
                                continue;
                            }
                            result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                            string message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                            Logging.Info("NOTIF", $"Recieved {message}");
                            ArraySegment<byte> responseBuffer = new ArraySegment<byte>(SocketServer.GenerateResponse());
                            await webSocket.SendAsync(responseBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                    catch (WebSocketException)
                    {
                        Logging.Warning("NOTIF", "Closed connection to websocket server.");
                    }
                    catch (Exception ex)
                    {
                        Logging.Error("NOTIF", $"Error: {ex.ToString()}");
                    }
                    finally
                    {
                        webSocket.Dispose();
                        Logging.Warning("NOTIF", "Disconnected from the websocket server.");
                    }
                }
            }
            listener.Close();
        }
        public static byte[] GenerateResponse()
        {
            int build = BuildLibrary.BuildId;
            if (build >= 42)
            {
                return Bytes.JsonObjectToByte(new { SessionId = 1 });
            }
            else
            {
                return Bytes.StringToByte("OK");
            }
        }
        public void Stop()
        {
            listening = false;
            this.listener.Close();
        }
    }
}
