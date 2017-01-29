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
            Group usersGroup;
            foreach(var group in groups)
            {
                if (group.Users.Contains(user))
                {
                    usersGroup = group;
                    return View(usersGroup);
                }
                else
                {

                }
            }

            //var group = _context.Groups.Include(g => g.Users).Single(g => g.Users.Contains(user));
            return RedirectToAction("NoGroup", "Customer");

            
        }
    }
}