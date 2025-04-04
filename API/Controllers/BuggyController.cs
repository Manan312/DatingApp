using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class BuggyController(DataContext dataContext) : BaseAPIController
    {
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetAuth()
        {
            return "search text";
        }
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = dataContext.Users.Find(Convert.ToInt64(-1));
            if (thing == null) return NotFound();
            return thing;
        }
        [HttpGet("server-error")]
        public ActionResult<AppUser> GetServerError()
        {
            var thing = dataContext.Users.Find(Convert.ToInt64(-1)) ?? throw new Exception("A bad thing has occurred");
            return thing;
        }
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was not a good request");
        }
    }
}