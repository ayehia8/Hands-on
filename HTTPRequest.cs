using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpClient
{
    class HTTPRequest
    {
        public static void HandleRequest(object obj)
        {
            string website = (string)obj;

            IPAddress[] addresses;

            try
            {
                addresses = Dns.GetHostAddresses(website);

                if (addresses.Length == 0)
                {
                    Console.WriteLine("Error: " + website);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            IPAddress host = addresses[0];
            int port = 80;

            IPEndPoint endpoint = new IPEndPoint(host, port);

            Socket socket = new Socket(AddressFamily.InterNetwork,
                                       SocketType.Stream,
                                       ProtocolType.Tcp);

            socket.Connect(endpoint);

            string request =
                "GET / HTTP/1.1\r\n" +
                "Host: " + website + "\r\n" +
                "Connection: close\r\n" +
                "\r\n";

            socket.Send(Encoding.ASCII.GetBytes(request));

            byte[] data = new byte[1024 * 1024];
            int received = socket.Receive(data);

            string response = Encoding.ASCII.GetString(data, 0, received);

            socket.Close();

            File.WriteAllText(website + ".html", response);

            Console.WriteLine("Finished: " + website);
        }
    }
}
