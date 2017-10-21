using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Shelly_Client
{
    class SocketHandler
    {
        public static byte[] Buffer;

        public static TcpClient client;

        public static SslStream sslStream;

        public static void Connect()
        {
            client = new TcpClient(Program.ServerIp, Program.ServerPort);
            sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
            sslStream.AuthenticateAsClient(Program.ServerIp);
            sslStream.ReadTimeout = 10000;
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static void Disconnect()
        {
            client.Close();
        }

        public static void Send(string Message)
        {
            sslStream.Write(Encoding.ASCII.GetBytes(" " + Message), 0, Message.Length + 1);
        }

        private static string TrimSpace;
        public static string Receive()
        {
            //---read back the text---
            Buffer = new byte[client.ReceiveBufferSize];

            for(int i = 0; i < 2; i++)
            {
                TrimSpace = Encoding.ASCII.GetString(Buffer, 0, sslStream.Read(Buffer, 0, Buffer.Length));
            }

            if (TrimSpace.Length == 0)
            {
                Disconnect();
            }

            return TrimSpace;
        }
    }
}
