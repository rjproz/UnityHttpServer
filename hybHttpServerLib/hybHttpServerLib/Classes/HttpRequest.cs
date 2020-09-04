using System;
using System.Collections.Generic;
using System.IO;
namespace Hybriona
{
    public class HttpRequest
    {
        public string host {get; private set;}
        public string method { get; private set; }
        public string uri { get; private set; }

        private Dictionary<string, string> headerList;

        private MemoryStream memoryStream;
        private bool isMethodParsed;
        private int consequentNewLineCounter;

        internal bool isDone = false;
        public HttpRequest()
        {
            memoryStream = new MemoryStream();
            headerList = new Dictionary<string, string>();
        }

        public bool HasHeader(string headerName)
        {
            return headerList.ContainsKey(headerName.ToUpper());
        }

        public string GetHeader(string headerName)
        {
            if (HasHeader(headerName.ToUpper()))
            {
                return headerList[headerName.ToUpper()];

            }
            else
            {
                throw new Exception("Specified Header key doesn't exist");
            }
        }
        public void Parse(byte [] data,int length)
        {
            for(int i=0; i < length; i++)
            {
                if(data[i] == '\n')
                {
                    consequentNewLineCounter++;
                    if (consequentNewLineCounter == 2)
                    {
                        //all headers are readed
                        isDone = true;
                        return;
                    }

                    //memoryStream.Write(data,0,i);
                   
                    if (!isMethodParsed)
                    {
                        string methodLine = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                        //Console.WriteLine(methodLine);
                        string [] parts = methodLine.Split(' ');

                        string uriMethod = parts[0];

                        if(uriMethod.ToUpper() != "GET")
                        {
                            throw new Exception("Only GET method is supported");
                        }


                        string uri = parts[1];

                        this.method = uriMethod;
                        this.uri = uri;
                        isMethodParsed = true;
                    }
                    else
                    {
                        string headerLine = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                        //Console.WriteLine(headerLine);
                        string[] parts = headerLine.Split(':');

                        string headerName = parts[0].ToUpper();
                        string headerValue = parts[1].Substring(1);

                        if(headerName.ToUpper() == "HOST")
                        {
                            host = headerValue;
                        }
                        headerList.Add(headerName, headerValue);
                    }

                    memoryStream.SetLength(0);
                    memoryStream.Position = 0;


                }
                else if(data[i] == '\r')
                {
                    //ignore
                }
                else
                {
                    consequentNewLineCounter = 0;
                    memoryStream.WriteByte(data[i]);
                }
            }
        }
       
    }
}
