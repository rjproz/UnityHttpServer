using System;
using Hybriona.Server;
namespace hybHttpServerTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            HttpServer httpServer = new HttpServer(81);
            httpServer.SetEventListener(OnHttpRequestReceived);
            httpServer.Start();
            Console.ReadKey();
            
        }

        static void OnHttpRequestReceived(HttpRequest httpRequest,HttpResponse response)
        {
            byte[] data = null;
            if (httpRequest.uri.Contains("hello"))
            {

                data = System.Text.Encoding.UTF8.GetBytes("thank u for saying hello to me!");
            }
            else
            {
                data = System.Text.Encoding.UTF8.GetBytes("how rude! u didn't greet me ;)");
            }
            response.responseStream.Write(data, 0, data.Length);
            response.Send(200);
        }
    }
}
