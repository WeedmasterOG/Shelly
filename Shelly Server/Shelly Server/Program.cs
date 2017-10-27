using System;
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
                Functions.ShowErrorMessage("ERROR: This program requires an active network connection to work", 5000);
                goto NetworkTryAgain;
            }

            // Get SSL input
            Functions.UserInput.GetSSLInput();

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

            // Set console title
            Console.Title = "Connected to " + SocketHandler.GetClientIp();

            // Display message
            Console.WriteLine("Client connected!");
            Thread.Sleep(2000);
            Console.Clear();

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
                    InputOutput.IO(UserInput);
                } catch
                {
                    // Shutdown server and disconnect
                    SocketHandler.ShutdownServer();
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
                if (SocketHandler.ConnectionStatus() == "false")
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

                // Check every 3 seconds
                Thread.Sleep(3000);
            }
        }
    }
}
