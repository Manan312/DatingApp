using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UserController(IUserRepository _userRepository,IMapper _mapper) : BaseAPIController
    {
        // private readonly DataContext _context;
        // public UserController(DataContext context)
        // {
        //     _context=context;
        // }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUser()
        {
            var users= await _userRepository.GetMembersAsync();
            //var users= await _userRepository.GetUsersAsync();

            // var usersToReturn =_mapper.Map<IEnumerable<MemberDTO>>(users);
            // return Ok(usersToReturn);
            return Ok(users);
        } 
        [HttpGet("{username}")] //api/user/id of user
        public async Task<ActionResult<MemberDTO>> GetUsers(string username)
        {
            var users=await _userRepository.GetMemberAsync(username); 
            //var users=await _userRepository.GetUserByUsernameAsync(username); 
            if(users==null)
            return NotFound();
            //var usersToReturn=_mapper.Map<MemberDTO>(users);
            return users;
        }
        [Authorize]
        [HttpGet("Authorize/{id:long}")] //api/user/id of user
        public async Task<ActionResult<MemberDTO>> AuthenticatedUser(Int64 id)
        {
            //var users=await _context.Users.FindAsync(id);
            var users=await _userRepository.GetUserByIdAsync(id);
            if(users==null)
            return NotFound("User Not Available");
            var usersToReturn=_mapper.Map<MemberDTO>(users);
            return usersToReturn;
        }
    }
}