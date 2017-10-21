using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;

namespace Shelly_Client
{
    class Functions
    {
        public static string ping()
        {
            using (Ping SendPing = new Ping())
            {
                return SendPing.Send(Program.ServerIp).RoundtripTime.ToString() + "ms";
            }
        }

        public static void GetClientInfo()
        {

        }
    }
}
