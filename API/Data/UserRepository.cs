using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository(DataContext context,IMapper _mapper) : IUserRepository
    {
        public async Task<MemberDTO?> GetMemberAsync(string username)
        {
            return await context.Users
            .Where(x=>x.UserName==username)
            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await context.Users
            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

        public async Task<AppUser?> GetUserByIdAsync(long id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await context.Users.Include(x=>x.Photos).SingleOrDefaultAsync(x=>x.UserName==username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Users
            .Include(x=>x.Photos)
            .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync()>0;
        }

        public void Update(AppUser User)
        {
            context.Entry(User).State=EntityState.Modified;
        }
    }
}