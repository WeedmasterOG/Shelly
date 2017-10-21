using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Shelly_Server
{
    class Program
    {
        public const string Ip = "127.0.0.1";
        public const int Port = 8500;

        public static Thread disconnectionHandler = new Thread(DisconnectionHandler);

        static void Main(string[] args)
        {
            TryAgain:
            Console.WriteLine("SSL certificate file path");

            try
            {
                SocketHandler.CertPath = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("SSL certificate file password");

                var Cpass = "";
                Cpass = Console.ReadLine();
                SocketHandler.CertPass = Cpass;
            } catch
            {
                Console.WriteLine("ERROR: Could not find SSL certificate file");
                Thread.Sleep(2000);
                Console.Clear();
                goto TryAgain;
            }



            if (!File.Exists(SocketHandler.CertPath))
            {
                Console.WriteLine("ERROR: Could not find SSL certificate file");
                Thread.Sleep(2000);
                Console.Clear();
                goto TryAgain;
            } else
            {
                if (Path.GetExtension(SocketHandler.CertPath) == ".pfx")
                {

                } else
                {
                    Console.WriteLine("ERROR: File is not an SSL certificate");
                    Thread.Sleep(2000);
                    Console.Clear();
                    goto TryAgain;
                }
            }


            Console.WriteLine("Listening...");

            SocketHandler.Listen();
            Console.Clear();

            SocketHandler.Connect();

            Console.WriteLine("Client connected!");
            Thread.Sleep(2000);
            Console.Clear();


            disconnectionHandler.Start();

            string UserInput;

            while (true)
            {
                Console.Write(">");
                UserInput = Console.ReadLine();
                Console.Write("\n");

                try
                {
                    switch (UserInput.ToLower())
                    {
                        case "help":
                            Console.WriteLine(
                                "1. Help - Displays this help section\n" + // None client
                                "2. Clear - Clears the console\n" + // None client
                                "3. Exit\n" +
                                "4. Uninstall - Uninstalls Shelly from the client computer\n" +
                                "5. Ping - Pings the client and shows the response"
                            );
                            break;

                        case "clear":
                            Console.Clear();
                            break;

                        case "exit":
                            SocketHandler.Disconnect();
                            ShowDisconnectMessage();
                            break;

                        case "uninstall":
                            SocketHandler.Send("uninstall");
                            SocketHandler.Disconnect();
                            ShowDisconnectMessage();
                            break;

                        case "ping":
                            SocketHandler.Send("ping");
                            Console.WriteLine(SocketHandler.Receive());
                            break;
                    }
                } catch
                {
                    SocketHandler.Disconnect();
                    ShowDisconnectMessage();
                }
            }
        }

        public static void DisconnectionHandler()
        {
            while(true)
            {
                if (SocketHandler.ConnectionStatus() == false)
                {
                    SocketHandler.Disconnect();

                    Console.Clear();
                    Console.WriteLine("Client disconnected");
                    Thread.Sleep(2500);
                    Environment.Exit(0);
                }

                Thread.Sleep(5000);
            }
        }

        public static void ShowDisconnectMessage()
        {
            Console.Clear();
            Console.WriteLine("Client disconnected");
            Thread.Sleep(2500);
            Environment.Exit(0);
        }
    }
}
