using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Entity
{
    internal class Log
    {
        public Int32 Id { get; set; }
        public Int32 UserId { get; set; }
        public DateTime LogIn { get; set; }
        public DateTime LogOut { get; set; }


        public Log(Int32 id, Int32 userId, DateTime logIn, DateTime logOut)
        {
            Id = id;
            UserId = userId;
            LogIn = logIn;
            LogOut = logOut;

        }
    }
}
