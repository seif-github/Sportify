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
using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;

namespace sportify.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountService(UserManager<ApplicationUser> userManager
            ,SignInManager<ApplicationUser> signInManager
            ,IMapper mapper)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._mapper = mapper;
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
                await _signInManager.SignInAsync(user, true);
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
            ApplicationUser user = await _userManager.FindByNameAsync(model.UserName);
            if(user != null)
            {
                bool checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
                if (checkPassword)
                {
                    await _signInManager.SignInAsync(user, model.RememberMe);
                    return "0"; // both are right
                }
                else
                {
                    return "1"; // password is wrong
                }
            }
            else
            {
                return "-1"; //username is wrong
            }
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
                    return setUsernameResult;
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
                // In a real app, you'd upload this to cloud storage or your file system
                // For simplicity, we'll just store the file name
                // Delete old image if exists
                if (!string.IsNullOrEmpty(user.ImageUrl))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", user.ImageUrl);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save new image
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                user.ImageUrl = fileName;
            }

            return await _userManager.UpdateAsync(user);
        }
    }
}
