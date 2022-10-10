using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace WerServer.Handlers;

//https://www.c-sharpcorner.com/uploadfile/bhushanbhure/websocket-server-using-httplistener-and-client-with-client/
public class WebSocketServer
{
    static HttpListener listener;

    public WebSocketServer()
    {
        if (!HttpListener.IsSupported)
        {
            throw new Exception("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
        }
        else
        {
            // Create a listener.
            listener = new HttpListener();
        }
        Console.WriteLine("Server Started...");
    }

    public async void Start(List<string> prefixes)
    {
        // Add the prefixes.
        foreach (string s in prefixes)
        {
            listener.Prefixes.Add(s);
        }
        listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        listener.Start();
        Console.WriteLine("Listening...");

        while (true)
        {
            HttpListenerContext context = await listener.GetContextAsync();
            if (context.Request.IsWebSocketRequest)
            {
                ProcessRequest(context);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }
    }

    private async void ProcessRequest(HttpListenerContext httpContext)
    {
        WebSocketContext context = null;
        try
        {
            context = await httpContext.AcceptWebSocketAsync(subProtocol: null);
            string ipAddress = httpContext.Request.RemoteEndPoint.Address.ToString();
        }
        catch (Exception ex)
        {
            httpContext.Response.StatusCode = 500;
            httpContext.Response.Close();
            Console.WriteLine("WebSocketContext Exception: {0}", ex);
            return;
        }

        WebSocket webSocket = context.WebSocket;
        try
        {
            byte[] receiveBuffer = new byte[1024];
            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                if (receiveResult.MessageType == WebSocketMessageType.Close)
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                else
                    await webSocket.SendAsync(new ArraySegment<byte>(receiveBuffer, 0, receiveResult.Count), WebSocketMessageType.Binary, receiveResult.EndOfMessage, CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("WebSocket Exception: {0}", ex);
        }
        finally
        {
            if (webSocket != null)
                webSocket.Dispose();
        }
    }
}
