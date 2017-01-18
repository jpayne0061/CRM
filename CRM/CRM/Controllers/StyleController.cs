using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Models;


namespace CRM.Controllers
{
    public class StyleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StyleController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Style
        public ActionResult Index()
        {


            return View();
        }

        //[ChildActionOnly]
        public string GetStyle()
        {

            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _context.Users.SingleOrDefault(u => u.Id == userId);

                return user.Style;
            }

            return "<link rel='stylesheet' href='/Content/flatly.min.css'>";
        }

        public ActionResult SetStyle(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            if(id == 1)
            {
                user.Style = "<link rel='stylesheet' href='/Content/darkly.min.css'>";
            }
            if (id == 2)
            {
                user.Style = "<link rel='stylesheet' href='/Content/ceru.min.css'>";
            }
            if (id == 3)
            {
                user.Style = "<link rel='stylesheet' href='/Content/flatly.min.css'>";
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Customer");
        }


    }
}