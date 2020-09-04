using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Hybriona
{

    public class HttpResponse
    {
        public MemoryStream responseStream { get; private set; }
        private MemoryStream headerStream;
       
        private NetworkStream networkStream;
        private Dictionary<string, string> headerList = new Dictionary<string, string>();
        public HttpResponse(NetworkStream stream)
        {
            networkStream = stream;
            headerStream = new MemoryStream();
            responseStream = new MemoryStream();
        }

        public void SetHeader(string headerName,string value)
        {
            headerName = headerName.ToUpper();
            if(headerList.ContainsKey(headerName))
            {
                headerList[headerName] = value;
            }
            else
            {
                headerList.Add(headerName, value);
            }
        }

        public void Send(int statusCode,string statusMessage = "")
        {
            byte[] header_data = System.Text.Encoding.UTF8.GetBytes(string.Format("HTTP/1.1 {0} {1}\r\n", statusCode, statusMessage));
            headerStream.Write(header_data, 0, header_data.Length);


            SetHeader("Content-Length", responseStream.Length.ToString());

            foreach(var header in headerList)
            {
                header_data = System.Text.Encoding.UTF8.GetBytes(string.Format("{0}: {1}\r\n",header.Key,header.Value));
                headerStream.Write(header_data, 0, header_data.Length);
            }

            header_data = System.Text.Encoding.UTF8.GetBytes("\r\n");
            headerStream.Write(header_data, 0, header_data.Length);

            if (headerStream.Length > 0)
            {
                byte [] data = headerStream.ToArray();
                networkStream.Write(data, 0, data.Length);
            }

            if (responseStream.Length > 0)
            {
                byte[] data = responseStream.ToArray();
                networkStream.Write(data, 0, data.Length);
            }

            headerStream = null;
            responseStream = null;
            headerList.Clear();
            headerList = null;
            networkStream.Close();
        }
    }
}