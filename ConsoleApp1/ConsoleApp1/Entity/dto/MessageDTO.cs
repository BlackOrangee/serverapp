using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Entity.dto
{
    internal class MessageDTO
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string MessageText { get; set; }
        public DateTime Time { get; set; }

        public MessageDTO(int id, User user, string messageText, DateTime time)
        {
            Id = id;
            User = user;
            MessageText = messageText;
            Time = time;
        }
    }
}
