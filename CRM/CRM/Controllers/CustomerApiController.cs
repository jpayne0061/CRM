using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRM.Models;

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



    }
}
