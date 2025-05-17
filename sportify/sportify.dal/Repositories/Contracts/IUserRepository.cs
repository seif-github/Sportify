using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sportify.DAL.Entities;

namespace sportify.DAL.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserById(string userId);
        Task<List<ApplicationUser>> GetUsersByIds(List<string> userIds);
    }
}
