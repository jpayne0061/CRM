using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRM.Models;
using Microsoft.AspNet.Identity;

namespace CRM.Content
{
    public class ChatMessagesApiController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public ChatMessagesApiController()
        {
            _context = new ApplicationDbContext();
        }



        [HttpGet]
        public IEnumerable<ChatMessage> GetMessages(string Id)
        {
            var receiverId = User.Identity.GetUserId();

            var chatMessages = _context.ChatMessages.Where(c => c.ReceiverId == receiverId && c.SenderId == Id && c.IsRead == false).ToList();

            if(chatMessages.Count > 0)
            {
                foreach (var chatMessage in chatMessages)
                {
                    chatMessage.IsRead = true;
                    _context.SaveChanges();
                }
            }

            return chatMessages; 
        }

        [HttpPost]
        public IHttpActionResult PostMessage([FromBody] ChatMessage chatMessage)
        {
            var sender = User.Identity.GetUserId();

            var message = new ChatMessage();

            message.Body = chatMessage.Body;
            message.IsRead = false;
            message.ReceiverId = chatMessage.ReceiverId;
            message.SenderId = sender;
            message.TimeStamp = DateTime.Now;


            _context.ChatMessages.Add(message);
            _context.SaveChanges();

            return Ok();
        }


    }
}
