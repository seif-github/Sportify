using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using sportify.BLL.DTOs;

namespace sportify.BLL.Services.Contracts
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterUserDTO model);
        Task<string> LoginUserAsync(LoginUserDTO model);
        Task LogoutUserAsync();
        Task<ProfileDTO> GetUserProfileAsync(string userId);
        Task<IdentityResult> UpdateUserProfileAsync(ProfileDTO model);
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword,
            string newPassword);
    }
}
