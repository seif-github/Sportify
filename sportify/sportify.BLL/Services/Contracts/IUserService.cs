using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sportify.BLL.DTOs;

namespace sportify.BLL.Services.Contracts
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetUsersByIds(List<string> userIds);
        Task<UserDTO> GetUserById(string userId);
    }
}
