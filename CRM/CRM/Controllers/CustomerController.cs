using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using CRM.ViewModels;
using Microsoft.AspNet.Identity;

namespace CRM.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var customers = _context.Customers.Include(c => c.Messages).ToList();

            return View(customers);
        }



        [Authorize]
        public ActionResult Create()
        {
            var users = _context.Users.Select(u => u.Name).ToList();

            var customerVm = new CustomerViewModel();

            customerVm.Users = new List<SelectListItem>();

            foreach (var name in users)
            {
                customerVm.Users.Add(new SelectListItem { Value = name, Text = name });
            }


            return View(customerVm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(CustomerViewModel viewCustomer)
        {
            if (!ModelState.IsValid)
            {
                
            }

            var userId = User.Identity.GetUserId();

            var creator = _context.Users.SingleOrDefault(u => u.Id == userId);

            var team = _context.Users.Include(u => u.UserNotifications).Where(u => viewCustomer.SelectedUsers.Contains(u.Name)).ToList();


            var customer = new Customer
            {
                Name = viewCustomer.Name,
                Phone = viewCustomer.Phone,
                Email = viewCustomer.Email,
                Team = team

            };


            //add notification for each team member that they have been added
            foreach(var user in team)
            {
                var userNotification = new UserNotification
                {
                    Sender = creator.Name,
                    CustomerName = customer.Name,
                    Body = creator.Name + " has assigned you to a team: " + customer.Name,
                    Recipient = user,
                    RecipientId = user.Id,
                    IsRead = false
                };


                user.UserNotifications.Add(userNotification);
                _context.UserNotifications.Add(userNotification);

            }

            _context.Customers.Add(customer);
            _context.SaveChanges();    

            return RedirectToAction("Index", "Customer");
        }



        public ActionResult Detail(int id)
        {

            var customer = _context.Customers.Include(c => c.Messages.Select(m => m.Author))
                                             .Include(c => c.Team)
                                             .Include(c => c.Tasks.Select(t => t.AssignedTo))
                                             .Include(c => c.Tasks.Select(t => t.AssignedBy))
                                             .SingleOrDefault(c => c.Id == id);

            var customerDetailVm = new CustomerDetailViewModel
            {
                Messages = customer.Messages.ToList(),
                Team = customer.Team.ToList(),
                Tasks = customer.Tasks.ToList(),
                Id = customer.Id,
                CurrentUserId = User.Identity.GetUserId(),
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email
            };

            return View(customerDetailVm);
        }


        [Authorize]
        public ActionResult AddTeam(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            var users = _context.Users.Select(u => u.Name).ToList();

            var customerVm = new CustomerViewModel();

            customerVm.Users = new List<SelectListItem>();
            customerVm.TransferId = customer.Id;

            foreach (var name in users)
            {
                customerVm.Users.Add(new SelectListItem { Value = name, Text = name });
            }

            return View(customerVm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddTeam(CustomerViewModel viewCustomer)
        {
            if (!ModelState.IsValid)
            {

            }

            var team = _context.Users.Where(u => viewCustomer.SelectedUsers.Contains(u.Name)).ToList();

            var customer = _context.Customers.SingleOrDefault(c => c.Id == viewCustomer.TransferId);

            customer.Team = team;

            _context.SaveChanges();

            return RedirectToAction("Index", "Customer");
        }

        [HttpGet]
        public ActionResult Edit(int id) {

            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            var users = _context.Users.Select(u => u.Name);

            var vm = new CustomerViewModel();

            vm.Users = new List<SelectListItem>();

            foreach(var name in users)
            {
                vm.Users.Add(new SelectListItem { Value = name, Text = name });
            }

            vm.EditId = id;
            vm.Phone = customer.Phone;
            vm.Name = customer.Name;
            vm.Email = customer.Email;

            return View(vm);
        }


        [HttpPost]
        public ActionResult Edit(CustomerViewModel vm) {

            var customer = _context.Customers.Include(c => c.Team).SingleOrDefault(c => c.Id == vm.EditId);

            var team = _context.Users.Where(u => vm.SelectedUsers.Contains(u.Name)).ToList();

            customer.Email = vm.Email;
            customer.Name = vm.Name;
            customer.Phone = vm.Phone;
            customer.Team = team;

            _context.SaveChanges();

            return RedirectToAction("Index", "Customer");
        }





    }
}