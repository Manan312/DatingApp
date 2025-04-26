using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UserController(IUserRepository _userRepository,IMapper _mapper, IPhotoServices _photoServices) : BaseAPIController
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
        [HttpPut("UpdateMemberData")]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            // var username=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // if(username==null)
            // return BadRequest("No Username found in token");

            var user=await _userRepository.GetUserByUsernameAsync(User.GetUserName());

            if(user==null) return BadRequest("No User Found");

            _mapper.Map(memberUpdateDTO,user);

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update the user");
        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile formFile)
        {
            var user= await _userRepository.GetUserByUsernameAsync(User.GetUserName());

            if(user==null)
            return BadRequest("Cannot update user");

            var result=await _photoServices.AddPhotoAsync(formFile);

            if(result.Error!=null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo=new Photo{
                Url=result.SecureUrl.AbsoluteUri,
                PublicId=result.PublicId
            };

            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync())
            return _mapper.Map<PhotoDTO>(photo);

            return BadRequest("Problem Adding the photo");
        }
    }
}