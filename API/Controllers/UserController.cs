using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context=context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUser()
        {
            var users= await _context.Users.ToListAsync();
            return users;
        } 
        [HttpGet("{id:long}")] //api/user/id of user
        public async Task<ActionResult<AppUser>> GetUsers(Int64 id)
        {
            var users=await _context.Users.FindAsync(id);
            if(users==null)
            return NotFound();
            return users;
        }
    }
}