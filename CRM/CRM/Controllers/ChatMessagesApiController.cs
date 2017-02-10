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

            var chatSession = _context.ChatSessions
                                      .Single(cs => (cs.ReceiverId == sender || cs.SenderId == sender) && cs.IsActive == true);

            chatSession.ChatMessages.Add(message);

            _context.ChatMessages.Add(message);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult StartChatSession([FromBody] ChatSession chatSessionId)
        {

            var userId = User.Identity.GetUserId();


            //check if user has joined chat inbetween time page loaded and first message sent

            var chatSession = new ChatSession
            {
                SenderId = userId,
                ReceiverId = chatSessionId.ReceiverId,
                RequesterName = _context.Users.Single(u => u.Id == userId).Name,
                RecipientName = _context.Users.Single(u => u.Id == chatSessionId.ReceiverId).Name
                
            };

            _context.ChatSessions.Add(chatSession);
            _context.SaveChanges();

            return Ok();
        }


        [HttpPatch]
        public IHttpActionResult EndChatSession()
        {
            var userId = User.Identity.GetUserId();

            var chatSession = _context.ChatSessions.Single(cs => (cs.ReceiverId == userId || cs.SenderId == userId) && cs.IsActive == true);

            chatSession.IsActive = false;

            _context.SaveChanges();

            return Ok();
        }



        [HttpGet]
        public List<ChatSession> GetChatSession()
        {
            var userId = User.Identity.GetUserId();

            var chatSessions = _context.ChatSessions.Where(cs => (cs.ReceiverId == userId || cs.SenderId == userId) && cs.IsActive == true).ToList();

            foreach (var chatSesh in chatSessions)
            {
                chatSesh.RequestingUser = userId;
            }

            return chatSessions;
        }

        [HttpGet]
        public List<ChatSession> GetPartnerSession(string id)
        {
            var userId = _context.Users.Single(u => u.Id == id).Id;

            var chatSessions = _context.ChatSessions.Where(cs => (cs.ReceiverId == userId || cs.SenderId == userId) && cs.IsActive == true).ToList();

            foreach (var chatSesh in chatSessions)
            {
                chatSesh.RequestingUser = userId;
            }

            return chatSessions;
        }




    }
}
