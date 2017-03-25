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

        public ActionResult CustomerFiles(int id)
        {
            var customer = _context.Customers.Include(c => c.CustomerFiles.Select(cf => cf.UploadedBy)).SingleOrDefault(c => c.Id == id);

            return View(customer);
        }


        public FileResult Download(string FileName)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(FileName);

            var namingconflict = FileName;

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = namingconflict,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            string contentType = MimeMapping.GetMimeMapping(FileName);
            return File(fileBytes, contentType);

        }

        //public FilePathResult Download(string FileName)
        //{
        //    //byte[] fileBytes = System.IO.File.ReadAllBytes(FileName);



        //    string contentType = MimeMapping.GetMimeMapping(FileName);

        //    return File(FileName, contentType);

        //}


        [HttpGet]
        public ActionResult Upload(int id)
        {
            var vm = new CustomerUploadVM();
            vm.CustomerId = id;

            return View(vm);
        }

        [HttpPost]
        public ActionResult Upload(CustomerUploadVM vm)
        {
            var customer = _context.Customers.Include(c => c.CustomerFiles).SingleOrDefault(c => c.Id == vm.CustomerId);
            try
            {
                if (vm.File.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(vm.File.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/CustomerFiles"), fileName);
                    vm.File.SaveAs(path);

                    

                    var customerFile = new CustomerFile();

                    var userId = User.Identity.GetUserId();
                    var user = _context.Users.SingleOrDefault(u => u.Id == userId);

                    customerFile.UploadedBy = user;
                    customerFile.DateTime = DateTime.Now;
                    customerFile.Customer = customer;
                    customerFile.FilePath = path;
                    customerFile.FileName = fileName;

                    _context.CustomerFiles.Add(customerFile);
                    customer.CustomerFiles.Add(customerFile);

                    _context.SaveChanges();
                }


                //ViewBag.Message = "Upload successful";

                //find customer by Id and add 'path' to files iCollection
                //return RedirectToAction("Index");


                return RedirectToAction("CustomerFiles", new { id=customer.Id });
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                return RedirectToAction("Uploads");
            }

            
        }


    }
}