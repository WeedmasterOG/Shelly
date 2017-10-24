using System;

namespace Shelly_Server
{
    class InputOutput
    {
        public static void IO(string UserIO)
        {
            // Turn to lower case and handle it
            switch (UserIO.ToLower())
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
                    SocketHandler.ShutdownServer();
                    break;

                // Uninstall shell
                case "uninstall":

                    // Send message
                    SocketHandler.Send("uninstall");

                    // Disconnect and exit
                    SocketHandler.ShutdownServer();
                    break;

                // Uninstall shell
                case "disconnect":

                    // Send message
                    SocketHandler.Send("disconnect");

                    // Disconnect and exit
                    SocketHandler.ShutdownServer();
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
                    SocketHandler.ShutdownServer();
                    break;

                // Request Ping from client
                case "restart":

                    // Send message
                    SocketHandler.Send("restart");

                    // Disconnect and exit
                    SocketHandler.ShutdownServer();
                    break;

                // Request Ping from client
                case "hibernate":

                    // Send message
                    SocketHandler.Send("hibernate");

                    // Disconnect and exit
                    SocketHandler.ShutdownServer();
                    break;
            }
        }
    }
}
