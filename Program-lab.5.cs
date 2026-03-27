using EchoServer;

class Program
{
    static void Main(string[] args)
    {
        Server server = new Server(8000);
        server.StartServer();
    }
}