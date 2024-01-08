using HRMS.solution.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.solution.Controllers
{
    public class AccountController : Controller
    {   
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        private string GenerateUsername(UserRegistrationViewModel model)
        {
            // Assuming the email is already validated
            return model.Email;
        }

        private string GenerateRandomPassword()
        {
            var options = userManager.Options.Password;

            string password = string.Empty;
            Random random = new Random();

            while (password.Length < 12)
            {
                char c = (char)random.Next(32, 126);
                if (IsPasswordCharValid(c, options))
                {
                    password += c;
                }
            }

            return password;
        }

        private bool IsPasswordCharValid(char c, PasswordOptions options)
        {
            return (!options.RequireDigit || char.IsDigit(c)) &&
                   (!options.RequireLowercase || char.IsLower(c)) &&
                   (!options.RequireUppercase || char.IsUpper(c)) &&
                   (!options.RequireNonAlphanumeric || !char.IsLetterOrDigit(c));
        }

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationViewModel userRegistrationViewMode)
        {
            if (ModelState.IsValid)
            {
                // Generate a username and a password
                var username = GenerateUsername(userRegistrationViewMode);
                var password = GenerateRandomPassword();

                var identityUser = new IdentityUser
                {
                    UserName = username,
                    Email = userRegistrationViewMode.Email,
                };

                var identityResult = await userManager.CreateAsync(identityUser, password);

                if (identityResult.Succeeded)
                {
                    // assign this user the "User" role
                    var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");

                    if (roleIdentityResult.Succeeded)
                    {
                        // Show success notification
                        return RedirectToAction("Register");
                    }

                    // If the creation fails, add the errors to the ModelState
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            // Show error notification
            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new UserLoginViewModel
            {
                ReturnUrl = ReturnUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var signInResult = await signInManager.PasswordSignInAsync(userLoginViewModel.Username,
                userLoginViewModel.Password, false, false);

            if (signInResult != null && signInResult.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(userLoginViewModel.ReturnUrl))
                {
                    return Redirect(userLoginViewModel.ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            //Show errors
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
