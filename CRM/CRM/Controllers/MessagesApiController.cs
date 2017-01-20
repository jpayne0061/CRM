using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRM.Models;
using Microsoft.AspNet.Identity;

namespace CRM.Controllers
{
    public class MessagesApiController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public MessagesApiController()
        {
            _context = new ApplicationDbContext();
        }

        public IHttpActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();

            var message = _context.Messages.SingleOrDefault(m => m.Id == id);

            if (!(userId == message.AuthorId))
            {
                //ERROR MESSAGE HERE

            }

            _context.Messages.Remove(message);
            _context.SaveChanges();

            return Ok();
        }



    }
}
