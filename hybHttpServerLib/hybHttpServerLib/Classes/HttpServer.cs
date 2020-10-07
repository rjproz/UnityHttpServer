using System;
using System.Net.Sockets;
using System.Net;
namespace Hybriona.Server
{
    public class HttpServer
    {
        public OnHttpRequestReceived onHttpRequestReceived;

        private int port;
        private TcpListener server;

        public HttpServer(int port)
        {
            this.port = port;
            server = new TcpListener(IPAddress.Any,this.port);
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
