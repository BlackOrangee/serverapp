using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

// SERVER CODE
namespace ConsoleApp2
{
    internal class Program
    {
        private static List<ClientInfo> clients = new List<ClientInfo>();
        private static readonly object lockObj = new object();

        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 25565);
            server.Start();
            Console.WriteLine("Server started");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Thread clientThread = new Thread(HandleClient);
                clientThread.Start(client);
            }
        }

        private static void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();
            string userName = "";

            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                userName = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                Console.WriteLine($"{userName} connected");

                ClientInfo clientInfo = new ClientInfo { Client = client, UserName = userName };

                lock (lockObj)
                {
                    clients.Add(clientInfo);
                }

                BroadcastMessage($"{userName} joined the chat.", client);

                while (true)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (message.StartsWith("/") || message == "exit")
                    {
                        break;
                    }

                    Console.WriteLine($"{userName}: {message}");

                    BroadcastMessage($"{userName}: {message}", client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                lock (lockObj)
                {
                    clients.RemoveAll(c => c.Client == client);
                }
                stream.Close();
                client.Close();
                Console.WriteLine($"{userName} disconnected");
                BroadcastMessage($"{userName} left the chat.", null);
            }
        }

        private static void BroadcastMessage(string message, TcpClient senderClient)
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(message);

            lock (lockObj)
            {
                foreach (ClientInfo client in clients)
                {
                    if (client.Client != senderClient)
                    {
                        NetworkStream stream = client.Client.GetStream();
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }
                }
            }
        }

        private class ClientInfo
        {
            public TcpClient Client { get; set; }
            public string UserName { get; set; }
        }
    }
}
