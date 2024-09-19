using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Entity.dto
{
    internal class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<Log> Logs { get; set; }
        public List<Message> Messages { get; set; }

        public UserDTO(int id, string name, string password, List<Log> logs, List<Message> messages)
        {
            Id = id;
            Name = name;
            Password = password;
            Logs = logs;
            Messages = messages;
        }
    }
}
