﻿using System;
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

            // Set console title
            Console.Title = "Disconnected";
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

        // GetClientIp method
        public static string GetClientIp()
        {
            // Get client ip
            IPEndPoint ClientIp = client.Client.RemoteEndPoint as IPEndPoint;

            // Set variables
            string input = ClientIp.ToString();
            int index = input.IndexOf(":");

            // Check if index is greater than 0
            if (index > 0)
            {
                // Trim string
                input = input.Substring(0, index);
            }

            // Return client ip
            return input;
        }

        // Send method
        public static void Send(string Message)
        {
            // Disable connection checking
            CheckConnection = false;

            // Write to the stream
            sslStream.Write(Encoding.ASCII.GetBytes(" " + Message), 0, Message.Length + 1);

            // Sleep for a short period of time to make sure the client receives the data
            Thread.Sleep(100);

            // Enable connection checking
            CheckConnection = true;
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
        public static bool CheckConnection = true;
        public static string ConnectionStatus()
        {
            // Try to send data to client
            try
            {
                // Check if CheckConnection is equal to true
                if (CheckConnection == true)
                {
                    // Write to the stream, send message manually to avoid cross thread errors
                    sslStream.Write(Encoding.ASCII.GetBytes(" " + "."), 0, ".".Length + 1);

                    // Return true if operation succeeded
                    return "true";
                } else
                {
                    // Return false if operation failed
                    return "nc";
                }
            }
            catch
            {
                // Return false, connection is down
                return "false";
            }
        }
    }
}
