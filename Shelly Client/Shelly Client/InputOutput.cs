using System;

namespace Shelly_Client
{
    class InputOutput
    {
        public static void IO(string IO)
        {
            // Read data
            switch (IO)
            {
                // Uninstall shell
                case "uninstall":

                    // Disconnect
                    SocketHandler.Disconnect();

                    // Uninstall
                    InstallFunctions.Uninstall();

                    // Exit
                    Environment.Exit(0);
                    break;


                // Send ping to server
                case "disconnect":

                    // Disconnect and exit
                    SocketHandler.ShutdownClient();
                    break;

                // Send ping to server
                case "ping":

                    // Send message
                    SocketHandler.Send(Functions.General.Ping());
                    break;

                // Send ping to server
                case "shutdown":

                    // Shutdown
                    Functions.PowerOptions.Shutdown();

                    // Disconnect and exit
                    SocketHandler.ShutdownClient();
                    break;

                // Send ping to server
                case "restart":
                    Functions.PowerOptions.Restart();

                    // Disconnect and exit
                    SocketHandler.ShutdownClient();
                    break;

                // Send ping to server
                case "hibernate":
                    Functions.PowerOptions.Hibernate();

                    // Disconnect
                    SocketHandler.Disconnect();
                    break;
            }
        }
    }
}
