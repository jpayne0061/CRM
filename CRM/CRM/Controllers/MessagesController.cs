using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM.ViewModels;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace CRM.Controllers
{
    public class MessagesController : Controller
    {

        private readonly ApplicationDbContext _context;

        public MessagesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Messages
        public ActionResult Create(int id)
        {
            var customer = _context.Customers.Include(c => c.Team).Single(c => c.Id == id);

            //this needs to chnage to query users associated with cutomer 
            //var users = _context.Users.Select(u => u.Name).ToList();
            var users = customer.Team.Select(u => u.Name);

            var messageVm = new MessageViewModel
            {
                CustomerId = id,
                CustomerName = customer.Name
            };

            messageVm.Users = new List<SelectListItem>();

            foreach(var name in users)
            {
                messageVm.Users.Add(new SelectListItem { Value = name, Text = name });
            }

            //messageVm.UserEmails = new List<SelectListItem>
            //{
            //    new SelectListItem { Value = "1", Text = "item 1" },

            //};

            return View(messageVm);
        }


        [HttpPost]
        [Authorize]
        public ActionResult Create(MessageViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }


            var customer = _context.Customers.Single(c => c.Id == vm.CustomerId);
            var userId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            var taggedUsers = _context.Users.Where(u => vm.SelectedEmails.Contains(u.Name)).ToList();

            var message = new Message
            {
                Body = vm.Body,
                Customer = customer,
                CustomerId = vm.CustomerId,
                AuthorId = userId,
                Author = user,
                AuthorName = user.Name,
                DateTime = DateTime.Now
            };

            _context.Messages.Add(message);
            _context.SaveChanges();

            foreach (var taggedUser in taggedUsers)
            {
                var userMessage = new UserMessage
                {
                    Message = message,
                    User = taggedUser
                };

                _context.UserMessages.Add(userMessage);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Customer");
        }

        public ActionResult UserMessages()
        {
            var userId = User.Identity.GetUserId();

            //var messages = _context.UserMessages.Include(m => m.Message).Where(m => m.UserId == userId).ToList();

            var messages = _context.UserMessages.Include(m => m.Message).Where(m => m.UserId == userId).Select(m => m.Message).Include(m => m.Customer).ToList();

            return View(messages);
        }

        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();

            var message = _context.Messages.Include(m => m.Customer.Team).Include(m => m.Author).SingleOrDefault(m => m.Id == id);

            var users = message.Customer.Team.Select(u => u.Name);

            if(!(userId == message.AuthorId))
            {
                //validate here or exclude 
            }

            var vm = new MessageViewModel
            {
                Body = message.Body,
                CustomerName = message.Customer.Name,
                CustomerId = message.Customer.Id,
                EditId = id
            };

            vm.Users = new List<SelectListItem>();

            foreach (var name in users)
            {
                vm.Users.Add(new SelectListItem { Value = name, Text = name });
            }

            return View(vm);
        }


        ////edit
        [HttpPost]
        [Authorize]
        public ActionResult Edit(MessageViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var customerVal = _context.Customers.SingleOrDefault(c => c.Id == vm.CustomerId);
                var users = customerVal.Team.Select(u => u.Name);
                vm.Users = new List<SelectListItem>();

                foreach (var name in users)
                {
                    vm.Users.Add(new SelectListItem { Value = name, Text = name });
                }



                return View("Edit", vm);
            }


            var customer = _context.Customers.Single(c => c.Id == vm.CustomerId);
            var userId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            var taggedUsers = _context.Users.Where(u => vm.SelectedEmails.Contains(u.Name)).Include(u => u.UserMessages.Select(um => um.Message)).ToList();

            var message = _context.Messages.SingleOrDefault(m => m.Id == vm.EditId);

            message.Body = vm.Body;
            message.Customer = customer;
            message.CustomerId = vm.CustomerId;
            message.AuthorId = userId;
            message.Author = user;
            message.AuthorName = user.Name;
            message.DateTime = DateTime.Now;
            

            _context.SaveChanges();

            foreach (var taggedUser in taggedUsers)
            {
                var userMessage = _context.UserMessages.SingleOrDefault(um => um.MessageId == vm.EditId);

                if(userMessage == null)
                {
                    var newUserMessage = new UserMessage
                    {
                        Message = message,
                        User = taggedUser
                    };

                    _context.UserMessages.Add(newUserMessage);
                }
                else
                {
                    userMessage.Message = message;
                }

               

                _context.SaveChanges();
            }

            return RedirectToAction("Detail", "Customer", new { id = customer.Id });
        }



    }
}