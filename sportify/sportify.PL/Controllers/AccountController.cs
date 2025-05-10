using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;

namespace sportify.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterUserAsync(model);

                if (result.Succeeded)
                    return RedirectToAction("Create", "League");
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("Summary", item.Description);
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserDTO model)
        {
            string loginResult = await _accountService.LoginUserAsync(model);
            if (loginResult.Equals("0"))
                return RedirectToAction("Index", "Home");
            else if (loginResult.Equals("1"))
                ModelState.AddModelError(string.Empty, "Passwrod is incorrect.\n Please try again.");
            else
                ModelState.AddModelError(string.Empty, "Account not found.\n Please register first.");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutUserAsync();
            return RedirectToAction("Index", "Home");
        }

        
    
    }
}
