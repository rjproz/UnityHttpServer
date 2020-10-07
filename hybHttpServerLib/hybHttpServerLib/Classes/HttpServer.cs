using System;
using System.Net.Sockets;
using System.Net;
namespace Hybriona.Server
{
    public class HttpServer
    {
        [ObsoleteAttribute("OnHttpRequestReceived is obsolete. Use onRequestReceived instead.", true)]
        public OnHttpRequestReceived onHttpRequestReceived;

        private int port;
        private TcpListener server;
        private System.Action<HttpRequest, HttpResponse> onRequestReceived;

        public HttpServer(int port)
        {
            this.port = port;
            this.server = new TcpListener(IPAddress.Any,this.port);
        }


        public void Start()
        {
            server.Start();
            server.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClientCallback), null);
        }

    
        public void Stop()
        {
            server.Stop();
        }

        public void SetEventListener(System.Action<HttpRequest, HttpResponse> action)
        {
            onRequestReceived = action;
        }

        public void EventCallback(HttpRequest headers, HttpResponse httpResponse)
        {
            if(onRequestReceived != null)
            {
                onRequestReceived(headers, httpResponse);
            }
        }


        private void AcceptTcpClientCallback(IAsyncResult result)
        {
            try
            {
                TcpClient tcpClient = server.EndAcceptTcpClient(result);
                new Client(tcpClient,this);
               
            }
            catch
            {

            }
            server.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClientCallback), null);
        }

    }
}
