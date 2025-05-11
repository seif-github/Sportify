using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Entities;

namespace sportify.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public AccountController(
            IAccountService accountService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _accountService = accountService;
            _userManager = userManager;
            _signInManager = signInManager;
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Sign out first
            await _signInManager.SignOutAsync();

            // Then delete
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            // If deletion failed
            TempData["Error"] = "Failed to delete account. Please try again.";
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return RedirectToAction("Login");

            var profile = await _accountService.GetUserProfileAsync(userId);
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileDTO model)
        {
            if (!ModelState.IsValid)
            {
                // Log model state errors for debugging
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(model);
            }

            var result = await _accountService.UpdateUserProfileAsync(model);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _accountService.ChangePasswordAsync
                (userId, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();

                TempData["SuccessMessage"] = "Password changed successfully!";
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
    }
}
