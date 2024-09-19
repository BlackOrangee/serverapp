using ConsoleApp1.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Entity;

namespace ConsoleApp1.Controller
{
    internal class UserController
    {
        private UserService userService = new UserService();
        public Response<int> RegisterUser(string username, string password)
        {
            int id = 0;
            Response<int> response = new Response<int>();

            try
            {
                id = userService.RegisterUser(username, password);
                response.Obj = id;
            }
            catch (Exception ex)
            {
                response.errorMessage = ex.Message;
            }

            return response;

        }

        public Response<int> LoginUser(string name, string password)
        {
            Response<int> response = new Response<int>();
            System.Console.WriteLine("Starting Login User Controller");

            try
            {
                response.Obj = userService.loginUser(name, password);
                System.Console.WriteLine("Login User Controller Success");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                response.errorMessage = ex.Message;
            }
            return response;
        }

        public void LogoutUser(int id) { userService.UserLogOut(id); }

        public Response<User> GetUserById(int id)
        {
            Response<User> response = new Response<User>();
            try
            {
                response.Obj = userService.GetUserById(id);
            }
            catch (Exception ex)
            {
                response.errorMessage = ex.Message;
            }
            return response;
        }

    }
}
