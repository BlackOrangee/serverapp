//using ConsoleApp1.Entity;
//using ConsoleApp1.Repository;
//using MySql.Data.MySqlClient;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ConsoleApp1.Service
//{
//    internal class LogService
//    {
//        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["db1_connectionString"].ConnectionString;

//        private UserRepository userRepository = new UserRepository();

//        private LogRepository logRepository = new LogRepository();

//        public int SaveLog(int userId, DateTime logIn, DateTime logOut)
//        {
//            int id = 0;

//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                connection.Open();
//                try
//                {
//                    id = logRepository.AddLog(connection, userId, logIn, logOut);

//                }
//                catch (Exception ex)
//                {
//                    throw new Exception("Database error.\nError:" + ex.Message);
//                }
//            }
//            return id;
//        }

//        public Log getLogsByUserId(Int32 userId)
//        {
//            Log log = null;
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                connection.Open();
//                try
//                {
//                    log = userRepository.getUserByEmail(connection, email);
//                }
//                catch (Exception ex)
//                {
//                    throw new Exception("Database error.\nError:" + ex.Message);
//                }
//            }

//            if (log == null)
//            {
//                throw new Exception("User with email: " + email + " not found");
//            }

//            role = roleService.getRoleById(userDTO.RoleId);

//            return new UserDTO(userDTO.Id, userDTO.Name, userDTO.Email, userDTO.Password, role);
//        }
      
//    }
//}
