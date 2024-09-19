using MySql.Data.MySqlClient;
using ConsoleApp1.Entity;


namespace ConsoleApp1.Repository
{
    internal class UserRepository
    {
        public int addUser(MySqlConnection connection, string name, string password)
        {
            string query = $"INSERT INTO users (name, password) VALUES (@name, @password); SELECT LAST_INSERT_ID();";
            int id = 0;
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@password", password);
                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return id;
        }


        public User getUserByName(MySqlConnection connection, string name)
        {
            User user = null;
            string query = $"SELECT * " +
                           $"FROM users " +
                           $"WHERE name = @name";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@name", name);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new User(
                            Convert.ToInt32(reader["id"]),
                            Convert.ToString(reader["name"]),
                            Convert.ToString(reader["password"]));
                    }
                }
            }
            return user;
        }

        public User getUserById(MySqlConnection connection, int id)
        {
            User user = null;
            string query = $"SELECT * " +
                           $"FROM users " +
                           $"WHERE id = @Id";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new User(
                            Convert.ToInt32(reader["id"]),
                            Convert.ToString(reader["name"]),
                            Convert.ToString(reader["password"]));
                    }
                }
            }
            return user;
        }

        public int updateUser(MySqlConnection connection, int id, string name, string password)
        {
            string query = $"UPDATE users " +
                           $"SET name = @Name, " +
                               $"password = @Password, " +
                           $"WHERE id = @Id";

            int result = 0;
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Id", id);

                result = cmd.ExecuteNonQuery();
            }
            return result;
        }

        public List<User> GetUsersByIds(MySqlConnection connection, List<int> ids)
        {
            List<User> users = new List<User>();
            
            string idsString = string.Join(",", ids);

            string query = $"SELECT * FROM users WHERE id IN ({idsString})";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User(
                            Convert.ToInt32(reader["id"]),
                            Convert.ToString(reader["name"]),
                            Convert.ToString(reader["password"])
                        );
                        users.Add(user);
                    }
                }
            }

            return users;
        }

    }
}
