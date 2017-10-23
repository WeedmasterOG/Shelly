using System;
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
                InstallFunctions.Cmd("timeout /t 2 & shutdown /s /t 0 /f");
            }
            
            // Restart method
            public static void Restart()
            {
                // Restart
                InstallFunctions.Cmd("timeout /t 2 & shutdown /r /t 0 /f");
            }

            // Hibernate method
            public static void Hibernate()
            {
                // Hibernate
                InstallFunctions.Cmd("timeout /t 2 & rundll32.exe powrprof.dll,SetSuspendState 0,1,0");
            }
        }
    }
}