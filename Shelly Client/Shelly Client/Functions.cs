using System;
using System.Net.NetworkInformation;

namespace Shelly_Client
{
    class Functions
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

        // GetClientInfo method
        public static void GetClientInfo()
        {

        }
    }
}