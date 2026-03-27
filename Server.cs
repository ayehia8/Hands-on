using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EchoServer
{
   class Server
    {
        private Socket serverSocket;
        private int port;

        public Server(int port)
        {
            this.port = port;

            serverSocket = new Socket(AddressFamily.InterNetwork,
                                      SocketType.Stream,
                                      ProtocolType.Tcp);

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
            serverSocket.Bind(endPoint);
        }

        public void StartServer()
        {
            serverSocket.Listen(100);
            Console.WriteLine("Server started...");

            while (true)
            {
                Socket clientSocket = serverSocket.Accept();

                Console.WriteLine("Client connected: " + clientSocket.RemoteEndPoint);

                Thread t = new Thread(new ParameterizedThreadStart(HandleConnection));
                t.Start(clientSocket);
            }
        }

        public void HandleConnection(object obj)
        {
            Socket client = (Socket)obj;

            string welcome = "Welcome to Echo Server\n";
            byte[] data = Encoding.ASCII.GetBytes(welcome);
            client.Send(data);

            while (true)
            {
                data = new byte[1024];
                int received = client.Receive(data);

                if (received == 0)
                {
                    Console.WriteLine("Client disconnected: " + client.RemoteEndPoint);
                    break;
                }

                string msg = Encoding.ASCII.GetString(data, 0, received);

                Console.WriteLine("Received: " + msg);

                // Echo back
                client.Send(data, 0, received, SocketFlags.None);
            }

            client.Close();
        }
    }
}
