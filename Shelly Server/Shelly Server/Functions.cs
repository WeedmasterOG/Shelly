using System;
using System.IO;
using System.Threading;

namespace Shelly_Server
{
    class Functions
    {
        // UserInput class
        public static class UserInput
        {
            public static void GetSSLInput()
            {
                TryAgain:

                // Get SSL cert file path
                Console.WriteLine("SSL certificate file path");

                // Get SSL cert file path
                SocketHandler.CertPath = Console.ReadLine();

                // Check if cert file exists
                if (!File.Exists(SocketHandler.CertPath))
                {
                    // Show error message
                    ShowErrorMessage("ERROR: Could not find SSL certificate file", 2000);
                    goto TryAgain;
                }
                else
                {
                    // Check if file is an actual cert file
                    if (Path.GetExtension(SocketHandler.CertPath) == ".pfx")
                    {

                    }
                    else
                    {
                        // Show error message
                        ShowErrorMessage("ERROR: File is not an SSL certificate", 2000);
                        goto TryAgain;
                    }
                }

                // Get SSL cert file pass
                Console.WriteLine("SSL certificate file password");
                SocketHandler.CertPass = Console.ReadLine();
            }

            // GetMessageBoxInoput method
            public static string GetMessageBoxInput()
            {
                // Display text
                Console.Write("Body: ");

                // Return user input
                return Console.ReadLine();
            }

            // GetDNEInput method
            public static string GetLinkInput(int Options)
            {
                TryAgain:

                // Switch
                switch(Options)
                {
                    // For DownloadAndExecute
                    case 1:

                        // Display text
                        Console.Write("Download Link(direct): ");
                        break;

                    // For ChangeWallpaper
                    case 2:

                        // Display text
                        Console.Write("Image download Link(direct, png & jpg only): ");
                        break;

                    // For OpenWebsite
                    case 3:

                        // Display text
                        Console.Write("Website Link: ");
                        break;
                }

                // Get user input
                string Url = Console.ReadLine();

                // Check if URL is valid
                if (Uri.IsWellFormedUriString(Url, UriKind.RelativeOrAbsolute) == false)
                {
                    // Display text
                    Console.WriteLine("ERROR: The URL you provided isnt valid\n");
                    goto TryAgain;
                }

                // Return user input
                return Url;
            }

            // GetExtentionInput method
            public static string GetExtentionInput()
            {
                // Display text
                Console.Write("File Extention(without dot): ");

                // Return data
                return Console.ReadLine();
            }

            // GetLockInput method
            public static string GetLockInput()
            {
                // Set int
                int Seconds;

                TryAgain:

                // Display text
                Console.Write("Seconds: ");

                // Try to get seconds
                try
                {
                    // Get input
                    Seconds = int.Parse(Console.ReadLine());

                } catch
                {
                    // Show error message
                    Console.WriteLine("ERROR: The input you provided isnt valid\n");
                    goto TryAgain;
                }

                // Check if Seconds is greater than 3600
                if (Seconds > 3600)
                {
                    // Show error message
                    Console.WriteLine("ERROR: You may not exeed 3600 seconds\n");
                    goto TryAgain;
                }

                // Return value
                return Seconds.ToString();
            }
        }

        // ShowErrorMessage method
        public static void ShowErrorMessage(string Message, int Delay)
        {
            // Display message
            Console.WriteLine(Message);

            // Sleep
            Thread.Sleep(Delay);

            // Clear console
            Console.Clear();
        }
    }
}
