using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Entity.dto
{
    internal class LogDTO
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime LogIn { get; set; }
        public DateTime LogOut { get; set; }


        public LogDTO(int id, User user, DateTime logIn, DateTime logOut)
        {
            Id = id;
            User = user;
            LogIn = logIn;
            LogOut = logOut;

        }
    }
}
