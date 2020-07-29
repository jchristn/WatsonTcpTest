using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WatsonTcp;

namespace WatsonTcpTest
{
    class Program
    {
        private static int Port = 42512;
        private static CompressionType CompType = CompressionType.Deflate;

        static void Main(string[] args)
        {
            try
            {
                var watsonServer = new WatsonTcpServer("127.0.0.1", Port)
                {
                    Compression = CompType,
                    Logger = Console.WriteLine,
                    DebugMessages = true
                };

                watsonServer.MessageReceived += (sender, message) =>
                {
                    Console.WriteLine("Server received message: " + Encoding.UTF8.GetString(message.Data));
                    watsonServer.Send(message.IpPort, message.Data);
                };

                watsonServer.Start();
                Task.Delay(1000).Wait();

                var client = new WatsonTcpClient("127.0.0.1", Port)
                {
                    Compression = CompType,
                    Logger = Console.WriteLine,
                    DebugMessages = true
                };
                 
                client.MessageReceived += (sender, message) =>
                {
                    Console.WriteLine("Client received message: " + Encoding.UTF8.GetString(message.Data)); 
                };

                client.Start();
                Task.Delay(1000).Wait();

                for (int i = 0; i < 10; i++)
                {
                    client.Send("Hello " + i);
                    Task.Delay(250).Wait();
                }

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        } 
    }
}