using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CRM.Models;

namespace CRM.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }


        public ActionResult Index()
        {

            var vm = new RegisterViewModel();

            var roles = _context.Roles.Where(u => !u.Name.Contains("Admin") && !u.Name.Contains("Admin2")).ToList();

            vm.Roles = roles;


            return View(vm);
        }

        public ActionResult ChatTest()
        {

            return View();
        }

        

    }
}