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
            var customer = _context.Customers.Single(c => c.Id == id);

            //this needs to chnage to query users associated with cutomer 
            var users= _context.Users.Select(u => u.Name).ToList();

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

            var customer = _context.Customers.Single(c => c.Id == vm.CustomerId);
            var userId = User.Identity.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            var taggedUsers = _context.Users.Where(u => vm.SelectedEmails.Contains(u.Email)).ToList();

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




    }
}