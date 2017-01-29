using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace CRM.Controllers
{
    public class JoinRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JoinRequestsController()
        {
            _context = new ApplicationDbContext();
        }


        // GET: JoinRequests
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var joinRequests = _context.JoinRequests.Include(jr => jr.Requester).Where(jr => jr.ManagerId == userId).ToList();

            return View(joinRequests);
        }
    }
}