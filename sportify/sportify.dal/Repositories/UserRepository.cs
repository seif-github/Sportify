using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sportify.DAL.Data;
using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;

namespace sportify.DAL.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly SportifyContext _context;
        private readonly DbSet<ApplicationUser> _dbSet;
        public UserRepository(SportifyContext context)
        {
            _context = context;
            _dbSet = _context.Set<ApplicationUser>();
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<ApplicationUser>> GetUsersByIds(List<string> userIds)
        {
            return await _dbSet.Where(u => userIds.Contains(u.Id)).ToListAsync();
        }
    }
}
