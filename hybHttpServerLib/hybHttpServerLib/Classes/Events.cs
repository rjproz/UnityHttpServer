using System;
namespace Hybriona.Server
{
    public delegate void OnHttpRequestReceived(HttpRequest headers, HttpResponse httpResponse);
}
