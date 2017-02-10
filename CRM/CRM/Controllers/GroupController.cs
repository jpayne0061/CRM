using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace CRM.Controllers
{
    public class GroupController : Controller
    {

        private readonly ApplicationDbContext _context;

        public GroupController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Group
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = _context.Users.Single(u => u.Id == userId);
            var groups = _context.Groups.Include(g => g.Users).ToList();

            var group = _context.Groups.Include(g => g.Users.Select(u => u.Tasks))
                                       .Include(g => g.Users.Select(u => u.Customers))
                                       .ToList().Single(g => g.Users.Contains(user));
            //.Select(g => g.Users.Contains(user));

            if (group != null) {
                return View(group);
            }
                
            
            //var group = _context.Groups.Include(g => g.Users).Single(g => g.Users.Contains(user));
            return RedirectToAction("NoGroup", "Customer");

            
        }
    }
}