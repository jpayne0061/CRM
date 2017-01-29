using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRM.Models;
using System.Data.Entity;

namespace CRM.Controllers
{
    public class CustomerApiController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public CustomerApiController()
        {
            _context = new ApplicationDbContext();
        }

        public IHttpActionResult Delete(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            _context.Customers.Remove(customer);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult DeleteTeamMember([FromBody] DeleteTeamMember tm)
        {
            var user = _context.Users.Single(u => u.Id == tm.TeamMemberId);
            var customer = _context.Customers.Include(c => c.Team).Single(c => c.Id == tm.CustomerId);

            customer.Team.Remove(user);
            _context.SaveChanges();

            return Ok();
        }



    }
}
