using ConsoleApp1.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Entity.dto;
using ConsoleApp1.Entity;
using MySql.Data.MySqlClient;

namespace ConsoleApp1.Service
{
    internal class MessageService
    {
        string connectionString = "Server=34.116.253.154;Port=3306;Database=chat_database;Uid=app_user;Pwd=&X9fT#7vYqZ$4LpR;";


        private UserRepository userRepository = new UserRepository();
        private MessageRepository messageRepository = new MessageRepository();


        public List<MessageDTO> GetAllMessages()
        {
            List<Message> messages = new List<Message>();
            List<User> users = new List<User>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    messages = messageRepository.GetAllMessagesInChronologicalOrder(connection);
                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }

                List<int> userIds = messages.Select(m => m.UserId).ToList();


                try
                {
                    users = userRepository.GetUsersByIds(connection, userIds);
                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }

            }

            List<MessageDTO> messagesWithUsers = new List<MessageDTO>();

            try
            {
                messages.ForEach(m =>
                {
                    messagesWithUsers.Add(new MessageDTO(m.Id, users.Find(u => u.Id == m.UserId), m.MessageText, m.Time));
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //messagesWithUsers.Reverse();


            return messagesWithUsers;
        }

        public int SaveMessage(int userId, string message, DateTime time)
        {
            int id = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    messageRepository.CreateMessage(connection, userId, message, time);

                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }
            return id;
        }
    }
}
