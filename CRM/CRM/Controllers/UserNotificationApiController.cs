using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRM.Models;
using Microsoft.AspNet.Identity;

namespace CRM.Controllers
{
    public class UserNotificationApiController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public UserNotificationApiController()
        {
            _context = new ApplicationDbContext();
        }


        [Authorize]
        public IEnumerable<UserNotificationDto> GetNotifications()
        {
            var userId = User.Identity.GetUserId();
            var notifications = _context.UserNotifications.Where(n => n.RecipientId == userId).ToList();
            var dtos = new List<UserNotificationDto>();


            foreach(var notification in notifications)
            {
                var dto = new UserNotificationDto
                {
                    Sender = notification.Sender,
                    CustomerName = notification.CustomerName,
                    Body = notification.Body,
                    IsRead = notification.IsRead
                };

                dtos.Add(dto);
            }
            return dtos;

        }

        [HttpDelete]
        public IHttpActionResult DeleteAll()
        {
            var userId = User.Identity.GetUserId();

            var notifications = _context.UserNotifications.Where(n => n.RecipientId == userId).ToList();

            var notificationContext = _context.UserNotifications;

            foreach (var notification in notifications)
            {
                notificationContext.Remove(notification);
            }

            _context.SaveChanges();

            return Ok();
        }




    }
}
