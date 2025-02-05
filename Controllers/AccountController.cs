using ChatRooms.Data;
using ChatRooms.Models;
using ChatRooms.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ChatRooms.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            var response = new AccountLoginViewModel();
            return View(response);
        }

        // POST: Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if (user != null)
            {
                // User is found, check password
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    // Password correct, sign in
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                // Password is incorrect
                TempData["Error"] = "Wrong credentials. Please, try again";
                return View(loginViewModel);
            }
            // User not found
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginViewModel);
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            var response = new AccountRegisterViewModel();
            return View(response);
        }

        // POST: Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager.FindByNameAsync(registerViewModel.UserName);
            if (user != null)
            {
                TempData["Error"] = "This username is already in use";
                return View(registerViewModel);
            }
            var newUser = new User()
            {
                UserName = registerViewModel.UserName
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                return RedirectToAction("Index", "Home");
            }

            // Logging runtime exception
            Log.Error("Exception occurred while trying to create user");
            return View(registerViewModel);
        }

        // POST: Account/Logout
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
