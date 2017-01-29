using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRM.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace CRM.Controllers
{
    public class JoinRequestApiController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public JoinRequestApiController()
        {
            _context = new ApplicationDbContext();
        }


        [Authorize]
        public IEnumerable<JoinRequest> GetJoinRequests()
        {
            var userId = User.Identity.GetUserId();

            var joinRequests = _context.JoinRequests.Where(jr => jr.ManagerId == userId).ToList();

            return joinRequests;

        }

        [Authorize]
        public IHttpActionResult Delete(int id)
        {

            var joinRequest = _context.JoinRequests.Single(jr => jr.Id == id);

            _context.JoinRequests.Remove(joinRequest);
            _context.SaveChanges();

            return Ok();

        }


        [Authorize]
        [HttpPost]
        public IHttpActionResult Accept([FromBody]int id)
        {

            var userId = User.Identity.GetUserId();

            var user = _context.Users.Include(u => u.Group).Single(u => u.Id == userId);

            var joinRequest = _context.JoinRequests.Include(jr => jr.Requester).Single(jr => jr.Id == id);

            var requester = joinRequest.Requester;

            //need to find out why there are two group columns

            var group = _context.Groups.Single(g => g.Name == user.Group.Name);
            


            group.Users.Add(requester);
            requester.Group = group;

            _context.JoinRequests.Remove(joinRequest);
            _context.SaveChanges();

            return Ok();

        }



    }
}
