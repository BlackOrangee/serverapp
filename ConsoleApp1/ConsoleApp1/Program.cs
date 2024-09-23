using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using ConsoleApp1.Controller;
using ConsoleApp1.Entity.dto;

namespace ConsoleApp2
{
    internal class Program
    {
        private static List<ClientInfo> clients = new List<ClientInfo>();
        private static readonly object lockObj = new object();
        private static MessageController messageController = new MessageController();
        private static UserController userController = new UserController();

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
            int userId = 0;

            try
            {
                while (userId == 0)
                {
                    //DirrectMessage("1 - Login\n2 - Register", client);

                    string response = ReadFromClient(client);

                    if(response == null || response == "")
                        continue;

                    string[] parts = response.Split(' ');

                    if (parts[0].Equals("Login"))
                    {
                        userId = HandleLogin(client, parts[1], parts[2]);
                    }else if (parts[0].Equals("Register"))
                    {
                        userId = HandleRegister(client, parts[1], parts[2]);
                    }

                    //switch (response)
                    //{
                    //    case "1":
                    //        userId = HandleLogin(client);
                    //        break;
                    //    case "2":
                    //        userId = HandleRegister(client);

                    //        break;
                    //    default:
                    //        DirrectMessage("Invalid input. Disconnecting...", client);
                    //        return;
                    //}
                }
                var getUserByIdResponse = userController.GetUserById(userId);

                if (!string.IsNullOrEmpty(getUserByIdResponse.errorMessage))
                {
                    DirrectMessage($"Error: {getUserByIdResponse.errorMessage}", client);
                }
                else
                {
                    userName = getUserByIdResponse.Obj.Name;
                }


                Console.WriteLine($"{userName} connected");

                ClientInfo clientInfo = new ClientInfo { Client = client, UserName = userName, Id = userId };

                lock (lockObj)
                {
                    clients.Add(clientInfo);
                }

                //Console.WriteLine("Start getting messages for connected");
                var aLLMessagesResponse = messageController.GetAllMessages();
                //Console.WriteLine("End getting messages for connected");

                if (!string.IsNullOrEmpty(aLLMessagesResponse.errorMessage))
                {
                    Console.WriteLine("Error ");

                    DirrectMessage(aLLMessagesResponse.errorMessage, client);
                }

                if (aLLMessagesResponse.Obj != null)
                {
                    Console.WriteLine("Start writing messages for connected");

                    List<MessageDTO> messages = aLLMessagesResponse.Obj;

                    foreach (MessageDTO item in messages)
                    {
                        Console.WriteLine($"{item.User.Name}: {item.MessageText} {item.Time}|");
                        DirrectMessage($"{item.User.Name}: {item.MessageText} {item.Time}|", client);
                    }

                    Console.WriteLine("End writing messages for connected");

                }


                BroadcastMessage($"{userName} joined the chat.", client);

                byte[] buffer = new byte[1024];
                int bytesRead;
                while (true)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    //if (message.StartsWith("/") || message == "exit")
                    //{
                    //    break;
                    //}
                    DateTime time = DateTime.Now;
                    messageController.SaveMessage(userId, message, time);

                    Console.WriteLine($"{userName}: {message} {time}");

                    BroadcastMessage($"{userName}: {message} {time}", client);
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
                BroadcastMessage($"{userName} left the chat.", client);
            }
        }

        private static int HandleLogin(TcpClient client, string username, string password)
        {

            //DirrectMessage("Enter username: ", client);
            //string enteredUsername = ReadFromClient(client);

            //DirrectMessage("Enter password: ", client);
            //string enteredPassword = ReadFromClient(client);

            var response = userController.LoginUser(username, password);

            if (!string.IsNullOrEmpty(response.errorMessage))
            {
                DirrectMessage($"Error {response.errorMessage}", client);
                return 0;
            }
            else
            {
                DirrectMessage("Login successful", client);
                return response.Obj;
            }
        }

        private static int HandleRegister(TcpClient client, string username, string password)
        {

            //DirrectMessage("Enter username: ", client);
            //string enteredUsername = ReadFromClient(client);

            //DirrectMessage("Enter password: ", client);
            //string enteredPassword = ReadFromClient(client);

            var response = userController.RegisterUser(username, password);

            if (!string.IsNullOrEmpty(response.errorMessage))
            {
                DirrectMessage($"Error {response.errorMessage}", client);
                return 0;
            }
            else
            {
                DirrectMessage("Registration successful", client);
                return response.Obj;
            }
        }

        private static string ReadFromClient(TcpClient client)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = client.GetStream().Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
        }

        private static void DirrectMessage(string message, TcpClient senderClient)
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(message);
            NetworkStream stream = senderClient.GetStream();
            stream.Write(responseBytes, 0, responseBytes.Length);
        }

        private static void BroadcastMessage(string message, TcpClient senderClient)
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(message);

            lock (lockObj)
            {
                foreach (ClientInfo client in clients)
                {
                    //if (client.Client != senderClient)
                    //{
                        NetworkStream stream = client.Client.GetStream();
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    //}
                }
            }
        }

        private class ClientInfo
        {
            public TcpClient Client { get; set; }
            public string UserName { get; set; }
            public int Id { get; set; }
        }
    }
}
