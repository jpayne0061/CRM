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


        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var user = _context.Users.Include(u => u.Customers.Select(c => c.Tasks)).Include(u => u.Group.Customers).Single(u => u.Id == userId);

            if (Request.IsAuthenticated &&  User.IsInRole("Manager")){
                var customers = user.Group.Customers.ToList();
                return View(customers);
            }

            if (user.Group != null)
            {
                var customers = user.Customers.ToList();

                return View(customers);
            }

            
            return RedirectToAction("NoGroup");
        }


        public ActionResult NoGroup() {


            return View();
        }

        public ActionResult WrongGroup()
        {


            return View();
        }

        public ActionResult WrongTeam()
        {
            var userId = User.Identity.GetUserId();

            var user = _context.Users.Include(u => u.Group.Manager).Single(u => u.Id == userId);

            return View(user);
        }


        [Authorize]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();

            var user = _context.Users.Include(u => u.Group.Users).Single(u => u.Id == userId);

            var users = user.Group.Users.Select(u => u.Name);

            var customerVm = new CustomerViewModel();

            customerVm.UserCheckBoxes = new List<UserCheckBox>();

            foreach (var name in users)
            {
                customerVm.UserCheckBoxes.Add(new UserCheckBox { Name = name, Checked = false});
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
            //build list of strings from checkbox
            List<string> usersSelected = new List<string>();

            foreach(UserCheckBox userCb in viewCustomer.UserCheckBoxes)
            {
                if(userCb.Checked == true)
                {
                    usersSelected.Add(userCb.Name);
                }
            }


            var userId = User.Identity.GetUserId();

            var creator = _context.Users.Include(u => u.Group.Users).SingleOrDefault(u => u.Id == userId);

            var group = creator.Group;

            var manager = _context.Users.Include(u => u.Customers).Single(u => u.Id == group.ManagerId);

            var groupUsers = group.Users.Select(u => u.Id).ToList();
            //var team = _context.Users.Where(u => group.Users.Contains(u)).Include(u => u.UserNotifications).Include(u => u.Customers).Where(u => usersSelected.Contains(u.Name)).Distinct().ToList();

            var team = _context.Users.Where(u => groupUsers.Contains(u.Id)).Include(u => u.UserNotifications).Include(u => u.Customers).Where(u => usersSelected.Contains(u.Name)).Distinct().ToList();

            var customer = new Customer
            {
                Name = viewCustomer.Name,
                Phone = viewCustomer.Phone,
                Email = viewCustomer.Email,
                Team = team,
                Group = group

            };

            _context.Customers.Add(customer);
            _context.SaveChanges();

            //add notification for each team member that they have been added
            foreach (var user in team)
            {
                var userNotification = new UserNotification
                {
                    Sender = creator.Name,
                    CustomerId = customer.Id,
                    CustomerName = customer.Name,
                    Body = creator.Name + " has assigned you to a team: " + customer.Name,
                    Recipient = user,
                    RecipientId = user.Id,
                    IsRead = false
                };

                user.Customers.Add(customer);
                user.UserNotifications.Add(userNotification);
                _context.UserNotifications.Add(userNotification);
                

            }


            //why is the below line adding the manager to customer's team?
            //manager.Customers.Add(customer);
            group.Customers.Add(customer);
            
            _context.SaveChanges();    

            return RedirectToAction("Index", "Customer");
        }


        [Authorize]
        public ActionResult Detail(int id)
        {

            var customer = _context.Customers.Include(c => c.Messages.Select(m => m.Author))
                                             .Include(c => c.Team)
                                             .Include(c => c.Group)
                                             .Include(c => c.Tasks.Select(t => t.AssignedTo))
                                             .Include(c => c.Tasks.Select(t => t.AssignedBy))
                                             .SingleOrDefault(c => c.Id == id);

            var userId = User.Identity.GetUserId();
            var user = _context.Users.Include(u => u.Group).Single(u => u.Id == userId);

            if(user.Group == null)
            {
                return RedirectToAction("NoGroup");
            }
            if(user.Group != customer.Group)
            {
                return RedirectToAction("WrongGroup");
            }
            if (!customer.Team.Contains(user))
            {
                return RedirectToAction("WrongTeam");
            }

            var customerDetailVm = new CustomerDetailViewModel
            {
                Messages = customer.Messages.ToList(),
                Team = customer.Team.ToList(),
                Tasks = customer.Tasks.OrderBy(t => t.IsComplete).ToList(),
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
            //look up current user
            //look up customer that will receive new team members
            //get all users in Group from group property of user
            //get all current team members from customer
            //get a list of team mates that are not already in the team

            var customer = _context.Customers.Include(c => c.Team).SingleOrDefault(c => c.Id == id);

            var userId = User.Identity.GetUserId();

            var user = _context.Users.Include(u => u.Group.Users).Single(u => u.Id == userId);

            var currentTeamNames = customer.Team.Select(u => u.Name).ToList();

            var users = user.Group.Users.Select(u => u.Name).Where(u => !currentTeamNames.Contains(u)).ToList();

            var customerVm = new CustomerViewModel();
            customerVm.Name = customer.Name;
            customerVm.TransferId = customer.Id;

            customerVm.UserCheckBoxes = new List<UserCheckBox>();

            foreach (var name in users)
            {
                customerVm.UserCheckBoxes.Add(new UserCheckBox { Name = name, Checked = false });
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
            List<string> usersSelected = new List<string>();

            foreach (UserCheckBox userCb in viewCustomer.UserCheckBoxes)
            {
                if (userCb.Checked == true)
                {
                    usersSelected.Add(userCb.Name);
                }
            }

            //////////////////////////

            var team = _context.Users.Where(u => usersSelected.Contains(u.Name)).ToList();

            var customer = _context.Customers.SingleOrDefault(c => c.Id == viewCustomer.TransferId);

            customer.Team = team;

            _context.SaveChanges();

            return RedirectToAction("Detail", "Customer", new { id = customer.Id });

        }

        [HttpGet]
        public ActionResult Edit(int id) {

            var customer = _context.Customers.Include(c => c.Team).SingleOrDefault(c => c.Id == id);

            var currentTeam = customer.Team.Select(t => t.Name);

            var userId = User.Identity.GetUserId();

            var user = _context.Users.Include(u => u.Group.Users).Single(u => u.Id == userId);

            var users = user.Group.Users.Select(u => u.Name);

            var vm = new CustomerViewModel();
            //
            vm.UserCheckBoxes = new List<UserCheckBox>();

            foreach (var name in users)
            {
                if (currentTeam.Contains(name))
                {
                    vm.UserCheckBoxes.Add(new UserCheckBox { Name = name, Checked = true });
                }
                else
                {
                    vm.UserCheckBoxes.Add(new UserCheckBox { Name = name, Checked = false });
                }
                
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

            List<string> usersSelected = new List<string>();

            foreach (UserCheckBox userCb in vm.UserCheckBoxes)
            {
                if (userCb.Checked == true)
                {
                    usersSelected.Add(userCb.Name);
                }
            }

            var team = _context.Users.Where(u => usersSelected.Contains(u.Name)).ToList();

            customer.Email = vm.Email;
            customer.Name = vm.Name;
            customer.Phone = vm.Phone;
            customer.Team = team;

            _context.SaveChanges();

            return RedirectToAction("Detail", "Customer", new { id = vm.EditId});
        }





    }
}