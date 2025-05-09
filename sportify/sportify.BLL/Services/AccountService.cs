using System;
using System.Collections.Generic;
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
            ,SignInManager<ApplicationUser> signInManager, IMapper mapper)
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
    }
}
