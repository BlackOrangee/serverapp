using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Entity
{
    public class Message
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MessageText { get; set; }
        public DateTime Time { get; set; }

        public Message(int id, int userId, string messageText, DateTime time)
        {
            Id = id;
            UserId = userId;
            MessageText = messageText;
            Time = time;
        }
    }
}
