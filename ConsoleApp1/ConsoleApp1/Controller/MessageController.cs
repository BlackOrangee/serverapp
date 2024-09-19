using ConsoleApp1.Entity;
using ConsoleApp1.Entity.dto;
using ConsoleApp1.Service;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Controller
{
    internal class MessageController
    {
        private MessageService messageService = new MessageService();

        public Response<List<MessageDTO>> GetAllMessages()
        {
            List<MessageDTO> messages = new List<MessageDTO>();
            Response<List<MessageDTO>> response = new Response<List<MessageDTO>>();

            try
            {
                messages = messageService.GetAllMessages();
                response.Obj = messages;
            }
            catch (Exception ex)
            {
                response.errorMessage = ex.Message;
            }

            return response;
        }

        public void SaveMessage(int userId, string message, DateTime time)
        {
            messageService.SaveMessage(userId, message, time);
        }
    }
}
