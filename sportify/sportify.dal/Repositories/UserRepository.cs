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
    public class UserRepository: GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context) { }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _context.Set<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<ApplicationUser>> GetUsersByIds(List<string> userIds)
        {
            return await _dbSet.Where(u => userIds.Contains(u.Id)).ToListAsync();
        }
    }
}
