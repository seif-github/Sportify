using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Data;
using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;

namespace sportify.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public AccountService(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IMapper mapper,
        IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterUserDTO model)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = model.UserName,
                PasswordHash = model.Password,
                Email = model.Email,
                ImageUrl = model.ImageUrl
            };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //await _signInManager.SignInAsync(user, true);
            }
            return result;
        }

        #region Return Logic regarding the Login 
        /*
         * return 1 if username is right but password is wrong
         * return 0 if username and password are both right
         * return -1 if username is wrong
         */
        #endregion
        public async Task<string> LoginUserAsync(LoginUserDTO model)
        {
            // Find user by username (since your DTO only has UserName)
            var user = await _userManager.FindByNameAsync(model.UserName);

            // User not found case
            if (user == null)
            {
                return "-1"; // User not found
            }

            // Email confirmation check
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return "3"; // Email not confirmed
            }

            var result = await _signInManager.PasswordSignInAsync(
            user.UserName,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false);

            return result.Succeeded ? "0" : "1"; // 0=Success, 1=Wrong password
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<ProfileDTO> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return _mapper.Map<ProfileDTO>(user);
        }

        public async Task<IdentityResult> UpdateUserProfileAsync(ProfileDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            // Update username if changed
            if (user.UserName != model.UserName)
            {
                var setUsernameResult = await _userManager.SetUserNameAsync(user, model.UserName);
                
                if (!setUsernameResult.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    return setUsernameResult;
                }
            }

            // Validate email format
            if (!new EmailAddressAttribute().IsValid(model.Email))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Invalid email address format" });
            }

            // Update email if changed
            if (user.Email != model.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                    return setEmailResult;
            }

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(user.ImageUrl))
                {
                    await _fileService.DeleteFileAsync(user.ImageUrl);
                }

                // Save new image
                user.ImageUrl = await _fileService.SaveFileAsync(model.ImageFile);
            }

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword,
            string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _userManager.ChangePasswordAsync(user,currentPassword,newPassword);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "User not found."
                });
            }

            if (result.Succeeded)
            {
                await _userManager.UpdateSecurityStampAsync(user);
            }
            return result;
        }
    }
}
