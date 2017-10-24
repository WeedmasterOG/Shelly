using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Shelly_Server
{
    class SocketHandler
    {
        public static string CertPath;
        public static string CertPass;
        private static IPAddress localAdd = IPAddress.Parse(Program.Ip);
        private static TcpListener listener = new TcpListener(localAdd, Program.Port);
        private static TcpClient client;
        private static byte[] Buffer;
        private static SslStream sslStream;

        // Listen method
        public static void Listen()
        {
            // Start listener
            listener.Start();

            // Accept incoming connection(client)
            client = listener.AcceptTcpClient();

            // Stop listener
            listener.Stop();
        }

        // Connect method
        public static void Connect()
        {
            // Create new sslstream instace and validate certificate
            sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

            // Create new certificate instance
            X509Certificate2 cert = new X509Certificate2(CertPath, CertPass);

            // Authenticate as server
            sslStream.AuthenticateAsServer(cert, false, SslProtocols.Tls, true);

            // Set receive timeout
            sslStream.ReadTimeout = 10000;
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

        // ShutdownServer method
        public static void ShutdownServer()
        {
            // Stop the disconnectionHandler thread
            Program.disconnectionHandler.Abort();

            // Disconnect
            Disconnect();

            // Display message
            Console.Clear();
            Console.WriteLine("Client disconnected");
            Thread.Sleep(2500);

            // Exit
            Environment.Exit(0);
        }

        // Send method
        public static void Send(string Message)
        {
            // Disable connection checking
            Program.CheckConnection = false;

            // Write to the stream
            sslStream.Write(Encoding.ASCII.GetBytes(" " + Message), 0, Message.Length + 1);

            // Enable connection checking
            Program.CheckConnection = true;
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

            // Return data received
            return TrimSpace;
        }

        // ConnectionStatus method
        public static bool ConnectionStatus()
        {
            // Try to send data to client
            try
            {
                // Write to the stream, send message manually ot avoid cross thread errors
                sslStream.Write(Encoding.ASCII.GetBytes(" " + "."), 0, ".".Length + 1);

                // Return true if operation succeeded
                return true;
            }
            catch
            {
                // Return false, connection is down
                return false;
            }
        }
    }
}
