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

            try
            {
                SocketHandler.Connect();
            } catch
            {
                Thread.Sleep(5000);
                goto TryToConnect;
            }
            
            while (true)
            {
                try
                {
                    switch (SocketHandler.Receive())
                    {
                        case "uninstall":
                            // Uninstall method here
                            break;

                        case "ping":
                            SocketHandler.Send(Functions.ping());
                            break;
                    }
                } catch
                {
                    SocketHandler.Disconnect();
                    goto TryToConnect;
                }
            }
        }
    }
}
