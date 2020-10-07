using System;
using System.Net.Sockets;
using System.Net;
namespace Hybriona.Server
{
    public class HttpServer
    {
        public System.Action<HttpRequest, HttpResponse> onRequestReceived { get; set; }

        [ObsoleteAttribute("OnHttpRequestReceived is obsolete. Use onRequestReceived instead.", true)]
        public OnHttpRequestReceived onHttpRequestReceived;

        private int port;
        private TcpListener server;
 
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
