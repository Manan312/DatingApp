using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly ITokenService _tokenService;
        private readonly DataContext _context;
        public AccountController(ITokenService tokenService, DataContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }
        [HttpPost("register")]//account/register
        public async Task<ActionResult<LoginReponseDTO>> Register(UserDTO Newuser)
        {
            using var hmac = new HMACSHA512();
            if (await UserExists(Newuser.UserName)) return BadRequest("UserName is Taken");
            // var user = new AppUser
            // {
            //     UserName = Newuser.UserName.ToLower(),
            //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Newuser.Password)),
            //     PasswordSalt = hmac.Key
            // };
            // _context.Users.Add(user);
            // await _context.SaveChangesAsync();
            // TokenDTO token = _tokenService.CreateToken(user);
            // if (token.Status == "F") return BadRequest(token.Message);
            // if (string.IsNullOrEmpty(token.Token)) return BadRequest("Error in Creating the JWT Token");
            // return new LoginReponseDTO
            // {
            //     UserName = user.UserName,
            //     Token = Convert.ToString(token.Token)
            // };
            return Ok("User Created");
        }
        [HttpPost("deleteUser")]
        public async Task<ActionResult<string>> DeleteUser(string Username)
        {
            try
            {
                var users = await _context.Users.FirstOrDefaultAsync(u => u.UserName == Username);
                if (users == null)
                {
                    return NotFound("No User with UserName " + Username + " Found!");
                }
                _context.Users.Remove(users);
                await _context.SaveChangesAsync();
                return Ok("User Removed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginReponseDTO>> Login(LoginRequestDTO loginDTO)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginDTO.Username);
                if (user == null) return NotFound("Invalid User name");

                using var hmac = new HMACSHA512(user.PasswordSalt);

                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
                for (int i = 0; i < computedHash.Count(); i++)
                {
                    if (computedHash[i] != user.PasswordHash[i])
                        return Unauthorized("Incorrect Login Details!!");
                }
                TokenDTO token = _tokenService.CreateToken(user);
                if (token.Status == "F") return BadRequest(token.Message);
                if (string.IsNullOrEmpty(token.Token)) return BadRequest("Error in Creating the JWT Token");
                return new LoginReponseDTO
                {
                    UserName = user.UserName,
                    Token = Convert.ToString(token.Token)
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, "User Not Found");
            }

        }
        private async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _context.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}