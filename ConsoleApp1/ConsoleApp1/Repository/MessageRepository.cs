using ConsoleApp1.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Repository
{
    public class MessageRepository
    {
        // Create a new message
        public void CreateMessage(MySqlConnection connection, int userId, string messageText, DateTime time)
        {
            string query = "INSERT INTO messages (user_id, message, message_time) VALUES (@UserId, @MessageText, @Time)";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@MessageText", messageText);
                cmd.Parameters.AddWithValue("@Time", time);

                cmd.ExecuteNonQuery();
            }
        }

        // Get message by ID
        public Message GetMessageById(MySqlConnection connection, int id)
        {
            Message message = null;
            string query = "SELECT * FROM messages WHERE id = @Id";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        message = new Message(
                            reader.GetInt32("id"),
                            reader.GetInt32("user_id"),
                            reader.GetString("message"),
                            reader.GetDateTime("time")
                        );
                    }
                }
            }
            return message;
        }

        // Get all messages by user ID
        public List<Message> GetMessagesByUserId(MySqlConnection connection, int userId)
        {
            List<Message> messages = new List<Message>();
            string query = "SELECT * FROM messages WHERE user_id = @UserId";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Message message = new Message(
                            reader.GetInt32("id"),
                            reader.GetInt32("user_id"),
                            reader.GetString("message"),
                            reader.GetDateTime("time")
                        );
                        messages.Add(message);
                    }
                }
            }
            return messages;
        }

        // Update message by ID
        public void UpdateMessage(MySqlConnection connection, Message message)
        {
            string query = "UPDATE messages SET message = @MessageText, time = @Time WHERE id = @Id";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Id", message.Id);
                cmd.Parameters.AddWithValue("@MessageText", message.MessageText);
                cmd.Parameters.AddWithValue("@Time", message.Time);

                cmd.ExecuteNonQuery();
            }
        }

        // Delete message by ID
        public void DeleteMessage(MySqlConnection connection, int id)
        {
            string query = "DELETE FROM messages WHERE id = @Id";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        // Get all messages in chronological order
        public List<Message> GetAllMessagesInChronologicalOrder(MySqlConnection connection)
        {
            List<Message> messages = new List<Message>();
            string query = "SELECT * FROM messages ORDER BY message_time ASC;";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Fetching messages...");
                    while (reader.Read())
                    {
                        Console.WriteLine("Processing message...");

                        Message message = new Message(
                            reader.GetInt32("id"),
                            reader.GetInt32("user_id"),
                            reader.GetString("message"),
                            reader.GetDateTime("message_time")
                        );
                        messages.Add(message);
                    }
                    Console.WriteLine("End fetching messages...");

                }
            }
            return messages;
        }
    }
}
