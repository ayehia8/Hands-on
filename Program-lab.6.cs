using System;
using System.Threading;
using HttpClient;

class Program
{
    static void Main(string[] args)
    {
        string[] websites = new string[]
        {
            "WWW.google.com",
            "www.gmail.com",
            "www.yahoo.com"
        };

        foreach (string website in websites)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(HTTPRequest.HandleRequest));
            thread.Start(website);
        }

        Console.ReadLine(); 
    }
}