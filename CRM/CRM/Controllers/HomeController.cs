using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CRM.Models;
using System.Data.Entity;
using CRM.ViewModels;
using System.IO;

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
            var userId = User.Identity.GetUserId();

            var user = _context.Users.Include(u => u.Group.Users).Single(u => u.Id == userId);

            var users = user.Group.Users;

            ChatUsersViewModel chatUsers = new ChatUsersViewModel();

            //add first             
            var userIds = _context.ChatSessions.Where(cs => cs.IsActive == true).Select(cs => cs.ReceiverId).ToList();
            var userIds2 = _context.ChatSessions.Where(cs => cs.IsActive == true).Select(cs => cs.SenderId).ToList();
            var userIdsCombined = userIds.Concat(userIds2).ToList();



            chatUsers.UsersNotAvailable = users.Where(u => userIdsCombined.Contains(u.Id)).ToList();
            chatUsers.UsersAvailable = users.Where(u => !userIdsCombined.Contains(u.Id)).ToList();
            chatUsers.CurrentUser = user;

            return View(chatUsers);
        }

        public ActionResult UploadDownload()
        {


            return View();
        }

        

    }
}