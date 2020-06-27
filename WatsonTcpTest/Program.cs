using System;
using System.Text;
using WatsonTcp;

namespace WatsonTcpTest
{
    class Program
    {
        private static int Port = 42512;

        static void Main(string[] args)
        {
            try
            {
                using (var server = new WatsonTcpServer("127.0.0.1", Port))
                {
                    server.ClientConnected += (s, client) => Console.WriteLine("Client connected");
                    server.ClientDisconnected += (s, client) => Console.WriteLine("Client connected");
                    server.MessageReceived += (s, message) => Console.WriteLine("Message received: " + Encoding.UTF8.GetString(message.Data));
                    server.Logger = Logger;
                    Console.WriteLine("Starting server...");
                    server.Start();

                    using (var client = new WatsonTcpClient("127.0.0.1", Port))
                    {
                        client.MessageReceived += (o, s) => Console.WriteLine("Message received");
                        client.Logger = Logger;
                        Console.WriteLine("Connecting client...");
                        client.Start();

                        if (!client.Connected) return;
                        else Console.WriteLine("Client connected");

                        Console.WriteLine("Sending data...");
                        client.Send("Test");
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static void Logger(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}