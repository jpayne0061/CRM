using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRM.Models;

namespace CRM.Controllers
{
    public class UsersController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public UsersController()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {

            var users = _context.Users.ToList();

            return users;

        }


    }
}
