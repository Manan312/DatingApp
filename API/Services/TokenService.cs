using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService:ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration=configuration;
        }
        public TokenDTO CreateToken(AppUser appUser)
        {
            var tokenDTO = new TokenDTO();
            try{
                var tokenKey=_configuration["TokenKey"];
                if(tokenKey==null)
                {
                    tokenDTO.Status="F";
                    tokenDTO.Message="The Token Key is empty!!!";
                    return tokenDTO;
                }
                if(tokenKey.Length<64)   
                {
                    tokenDTO.Status="F";
                    tokenDTO.Message="Token Key not upto required length!!!";
                    return tokenDTO;
                }
                var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

                var claims=new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,appUser.UserName)
                };

                var creds= new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor=new SecurityTokenDescriptor{
                    Subject=new ClaimsIdentity(claims),
                    Expires=DateTime.UtcNow.AddDays(7),
                    SigningCredentials=creds
                };

                var tokenHandler=new JwtSecurityTokenHandler();

                var token=tokenHandler.CreateToken(tokenDescriptor);

                tokenDTO.Status="S";
                tokenDTO.Message="Success";
                tokenDTO.Token=tokenHandler.WriteToken(token);
            }
            catch(Exception ex)
            {
                tokenDTO.Status="F";
                tokenDTO.Message=ex.Message;
            }
            return tokenDTO;
        }
    }
}