# Unity and Mono Compatible Simple HttpServer Library

**Currently only supports GET method**


## Unity Sample
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hybriona;
public class ServerTest : MonoBehaviour
{
    // Start is called before the first frame update
    private HttpServer httpServer;
    void Start()
    {
        httpServer = new HttpServer(8081);
        httpServer.Start();

        httpServer.onHttpRequestReceived += OnHttpRequestReceived;
    }

    private void OnHttpRequestReceived(HttpRequest httpRequest, HttpResponse response)
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

    private void OnDisable()
    {
        if (httpServer != null)
        {
            httpServer.Stop();
            httpServer.onHttpRequestReceived = null;
        }
    }
}
```

## Mono Sample

```csharp
using System;
using Hybriona;
namespace hybHttpServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer httpServer = new HttpServer(81);
            httpServer.Start();
            httpServer.onHttpRequestReceived += OnHttpRequestReceived;
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

```