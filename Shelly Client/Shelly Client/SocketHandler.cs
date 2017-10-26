using System;
using System.Text;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Linq;

namespace Shelly_Client
{
    class SocketHandler
    {
        private static byte[] Buffer;
        private static TcpClient client;
        private static SslStream sslStream;

        // Connect method
        public static void Connect()
        {
            // Create new instance
            client = new TcpClient(Program.ServerIp, Program.ServerPort);

            // Create new sslstream instace and validate certificate
            sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

            // Authenticate as client
            sslStream.AuthenticateAsClient(Program.ServerIp);

            // Set receive timeout
            sslStream.ReadTimeout = 12000;
        }

        // ValidateServerCertificate method
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        // Disconnect method
        public static void Disconnect()
        {
            // Close socket
            client.Close();
        }

        // ShutdownClient method
        public static void ShutdownClient()
        {
            // Disconnect
            Disconnect();

            // Exit
            Environment.Exit(0);
        }

        // Send method
        public static void Send(string Message)
        {
            // Write to the stream
            sslStream.Write(Encoding.ASCII.GetBytes(" " + Message), 0, Message.Length + 1);
        }

        // Receive method
        private static string TrimSpace;
        public static string Receive()
        {
            // Setup buffer
            Buffer = new byte[client.ReceiveBufferSize];

            // Read incoming data and strip front space
            for (int i = 0; i < 2; i++)
            {
                TrimSpace = Encoding.ASCII.GetString(Buffer, 0, sslStream.Read(Buffer, 0, Buffer.Length));
            }

            // Check for graceful disconnect
            if (TrimSpace.Length == 0)
            {
                Disconnect();
            }

            // Return data received
            return TrimSpace;
        }
    }
}
