using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.Models;
using CRM.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace CRM.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Tasks
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserTasks()
        {

            var userId = User.Identity.GetUserId();
            var tasks = _context.Tasks.Include(t => t.Customer).Include(t => t.AssignedBy).Where(t => t.AssignedToId == userId).ToList();
            var username = _context.Users.SingleOrDefault(u => u.Id == userId).Name;

            ViewBag.Username = username;

            return View(tasks);
        }


        [Authorize]
        public ActionResult Create(int id)
        {
            var taskMaker = User.Identity.GetUserId();

            var users = _context.Users.ToList();

            var taskVm = new TaskViewModel
            {
                CustomerId = id,
                Users = users
            };


            return View(taskVm);
        }


        [HttpPost]
        [Authorize]
        public ActionResult Create(TaskViewModel vm)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == vm.CustomerId);
            var assignedTo = _context.Users.SingleOrDefault(u => u.Id == vm.AssignedTo);
            var userId = User.Identity.GetUserId();
            var assignedBy = _context.Users.SingleOrDefault(u => u.Id == userId);

            var task = new Task
            {
                Customer = customer,
                CustomerId = vm.CustomerId,
                AssignedTo = assignedTo,
                AssignedToId = assignedTo.Id,
                AssignedBy = assignedBy,
                AssignedById = userId,
                Deadline = vm.Deadline,
                Body = vm.Body
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return RedirectToAction("Index", "Customer");
        }


    }
}