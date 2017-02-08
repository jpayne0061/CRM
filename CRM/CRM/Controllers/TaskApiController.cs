using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRM.Models;

namespace CRM.Controllers
{
    public class TaskApiController : ApiController
    {

        private readonly ApplicationDbContext _context;

        public TaskApiController()
        {
            _context = new ApplicationDbContext();
        }

        public IHttpActionResult Delete(int id)
        {

            var task = _context.Tasks.SingleOrDefault(t => t.Id == id);


            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut]
        public IHttpActionResult MarkComplete(int id)
        {
            var task = _context.Tasks.SingleOrDefault(t => t.Id == id);

            task.IsComplete = true;

            _context.SaveChanges();

            return Ok();
        }


    }
}
