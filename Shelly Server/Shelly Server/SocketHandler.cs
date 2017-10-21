using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Shelly_Server
{
    class SocketHandler
    {
        public static string CertPath;
        public static string CertPass;
        public static IPAddress localAdd = IPAddress.Parse(Program.Ip);
        public static TcpListener listener = new TcpListener(localAdd, Program.Port);
        public static TcpClient client;
        public static byte[] Buffer;
        public static SslStream sslStream;

        public static void Listen()
        {
            listener.Start();
            client = listener.AcceptTcpClient();
            listener.Stop();
        }

        public static void Connect()
        {
            sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
            X509Certificate2 cert = new X509Certificate2(CertPath, CertPass);
            sslStream.AuthenticateAsServer(cert, false, SslProtocols.Tls, true);
            sslStream.ReadTimeout = 10000;
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static void Disconnect()
        {
            Program.disconnectionHandler.Abort();
            client.Close();
        }

        public static void Send(string Message)
        {
            sslStream.Write(Encoding.ASCII.GetBytes(" " + Message), 0, Message.Length + 1);
        }

        private static string TrimSpace;
        public static string Receive()
        {
            Buffer = new byte[client.ReceiveBufferSize];

            for (int i = 0; i < 2; i++)
            {
                TrimSpace = Encoding.ASCII.GetString(Buffer, 0, sslStream.Read(Buffer, 0, Buffer.Length));
            }

            return TrimSpace;
        }

        public static bool ConnectionStatus()
        {
            try
            {
                Send(".");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
