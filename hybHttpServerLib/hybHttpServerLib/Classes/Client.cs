using System;
using System.IO;
using System.Net.Sockets;

namespace Hybriona.Server
{
    public class Client : IDisposable
    {
        private byte[] buffer;
        private TcpClient client;
        private HttpServer assocServer;
        private NetworkStream networkStream;
        private HttpRequest headers;
        public Client(TcpClient client,HttpServer assocServer )
        {
            buffer = new byte[512];
            this.client = client;
            this.assocServer = assocServer;
            this.networkStream = client.GetStream();
            this.headers = new HttpRequest();

            this.networkStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(BeginReadCacllback), null);
           
        }

        ~ Client()
        {
            Dispose();
        }

        public void Dispose()
        {
            try
            {
                buffer = null;
                headers = null;
                if(networkStream != null)
                {
                    networkStream.Close();
                }

                if(client != null)
                {
                    client.Close();
                    client.Dispose();
                }
            }
            catch
            {

            }
        }

        private void BeginReadCacllback(IAsyncResult result)
        {
            int count = networkStream.EndRead(result);
           
           
            if(count > 0)
            {
                headers.Parse(buffer, count);
                if(headers.isDone)
                {
                    assocServer.EventCallback(headers, new HttpResponse(networkStream));
                }
                else
                {
                    this.networkStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(BeginReadCacllback), null);
                }
            }
            else
            {
                Dispose();
            }

        }

       
    }
}
