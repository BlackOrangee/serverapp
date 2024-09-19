using ConsoleApp1.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Repository;

namespace ConsoleApp1.Service
{
    internal class UserService
    {
        string connectionString = "Server=34.116.253.154;Port=3306;Database=chat_database;Uid=app_user;Pwd=&X9fT#7vYqZ$4LpR;";

        private UserRepository userRepository = new UserRepository();
        private LogRepository logRepository = new LogRepository();

        public int RegisterUser(string username, string password)
        {
            int id = 0;
            User user = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    user = userRepository.getUserByName(connection, username);

                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }

                if (user != null)
                {
                    throw new Exception("User with name: " + username + " already exists");
                }

                try
                {
                    id = userRepository.addUser(connection, username, password);
                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }
            return id;
        }

        public User getUserByName(string name)
        {
            User user = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    user = userRepository.getUserByName(connection, name);
                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }

            if (user == null)
            {
                throw new Exception("User with name: " + name + " not found");
            }


            return user;
        }

        public int loginUser(string name, string password)
        {
            User user = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                System.Console.WriteLine("User service, starting login");
                try
                {
                    user = userRepository.getUserByName(connection, name);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Error: " + ex.Message);
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }

            if (user == null)
            {
                System.Console.WriteLine("User with name: " + name + " not found");
                throw new Exception("User with name: " + name + " not found");
            }

            if (!user.Password.Equals(password))
            {
                System.Console.WriteLine("Incorrect password");
                throw new Exception("Incorrect password");
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
               
                try
                {
                    int id = logRepository.AddLog(connection, user.Id, DateTime.Now);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Error: " + ex.Message);
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }
            return user.Id;
        }


        public void UserLogOut(int userId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    int id = logRepository.UpdateLog(connection, userId, DateTime.Now);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Error: " + ex.Message);
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }
        }

        public User GetUserById(int id)
        {
            User user = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    user = userRepository.getUserById(connection, id);
                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }

            if (user == null)
            {
                throw new Exception("User with id: " + id + " not found");
            }

            return user;
        }

        public int updateUser(string name, string password)
        {
            User user = getUserByName(name);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    return userRepository.updateUser(connection, user.Id, name, password);
                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }
        }
    }
}
