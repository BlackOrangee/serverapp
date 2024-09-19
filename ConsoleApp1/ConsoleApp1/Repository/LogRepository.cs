using ConsoleApp1.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1.Repository
{
    internal class LogRepository
    {
        public int AddLog(MySqlConnection connection, int user_id, DateTime log_in)
        {
            string query = "INSERT INTO logs (user_id, log_in) VALUES (@userId, @logIn); SELECT LAST_INSERT_ID();";
            int id = 0;
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@userId", user_id);
                cmd.Parameters.AddWithValue("@logIn", log_in);
                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return id;
        }

        public int UpdateLog(MySqlConnection connection, int user_id, DateTime log_out)
        {
            string query = @"
        UPDATE logs 
        SET log_out = @logOut 
        WHERE id = (
            SELECT id 
            FROM (
                SELECT id 
                FROM logs 
                WHERE user_id = @userId AND log_out IS NULL
                ORDER BY log_in DESC
                LIMIT 1
            ) AS temp
        )";

            int rowsAffected = 0;

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@userId", user_id);
                cmd.Parameters.AddWithValue("@logOut", log_out);

                rowsAffected = cmd.ExecuteNonQuery();
            }

            return rowsAffected;
        }



        public List<Log> GetAllLogs(MySqlConnection connection)
        {
            string query = "SELECT id, user_id, log_in, log_out FROM logs";
            List<Log> logs = new List<Log>();

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Log log = new Log
                        (
                            reader.GetInt32("id"),
                            reader.GetInt32("user_id"),
                            reader.GetDateTime("log_in"),
                            reader.GetDateTime("log_out")
                        );
                        logs.Add(log);
                    }
                }
            }
            return logs;
        }

        public Log getLogById(MySqlConnection connection, int id)
        {
            Log log = null;
            string query = $"SELECT * " +
                           $"FROM logs " +
                           $"WHERE id = @Id";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        log = new Log(
                            reader.GetInt32("id"),
                            reader.GetInt32("user_id"),
                            reader.GetDateTime("log_in"),
                            reader.GetDateTime("log_out")
                            );
                    }
                }
            }
            return log;
        }

        public List<Log> getLogsByUserId(MySqlConnection connection, int userId)
        {
            List<Log> logs = new List<Log>();
            string query = $"SELECT * " +
                           $"FROM logs " +
                           $"WHERE user_id = @UserId";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Log log = new Log(
                            reader.GetInt32("id"),
                            reader.GetInt32("user_id"),
                            reader.GetDateTime("log_in"),
                            reader.GetDateTime("log_out")
                        );
                        logs.Add(log);
                    }
                }
            }
            return logs;
        }

    }
}
