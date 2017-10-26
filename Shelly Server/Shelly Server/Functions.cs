using System;
using System.IO;
using System.Threading;

namespace Shelly_Server
{
    class Functions
    {
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
