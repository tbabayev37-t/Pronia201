using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCProniaTask.Abstractions;
using MVCProniaTask.Contexts;
using MVCProniaTask.ViewModels.UserViewModels;

namespace MVCProniaTask.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager, IConfiguration _configuration, IEmailService _emailService) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>  Register(RegisterVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }
            var existUser = await _userManager.FindByNameAsync(vm.UserName);
            if(existUser is { })
            {
                ModelState.AddModelError("Username", "This user is already exist");
                return View(vm);
            }
            existUser = await _userManager.FindByEmailAsync(vm.EmailAddress);
            if(existUser is { })
            {
                ModelState.AddModelError("EmailAddress", "This email is already exist");
                return View(vm);
            }
            AppUser user = new()
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.EmailAddress,
                UserName = vm.UserName,
            };
         var result = await   _userManager.CreateAsync(user, vm.Password);
            if(result.Succeeded == false)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(vm);
            }
            await _userManager.AddToRoleAsync(user, "Member");

           

            await SendConfirmEmailAsync(user);
            TempData["SuccessMessage"] = "Please confirm your email";
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>  Login(LoginVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = await _userManager.FindByEmailAsync(vm.EmailAddress);
            if(user is null)
            {
                ModelState.AddModelError("", "Email or Password is wrong");
                return View(vm);
            }
            var result = await _userManager.CheckPasswordAsync(user, vm.Password);
            if(result == false)
            {
                ModelState.AddModelError("", "Email or Password is wrong");
                return View(vm);
            }
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Please confirm your email address");
                await SendConfirmEmailAsync(user);
                return View(vm);
            }

            await _signInManager.SignInAsync(user, vm.IsRemember);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult>  LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult>  CreateRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Admin"
            });
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Member"
            });
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Moderator"
            });
            return Ok("Roles created");
        }

        public async Task<IActionResult> CreateAdminAndModerator()
        {
            /*AppUser adminUser = new()
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                FirstName = "Admin",
                LastName = "System"
            };

            AppUser moderatorUser = new()
            {
                UserName = "moderator",
                Email = "moderator@gmail.com",
                FirstName = "Moderator",
                LastName = "System"
            };*/
            var adminUserVm = _configuration.GetSection("AdminUser").Get<UserVM>();
            var moderatorUserVm = _configuration.GetSection("ModeratorUser").Get<UserVM>();

            if(adminUserVm is not null)
            {
                AppUser adminUser = new()
                {
                    FirstName = adminUserVm.FirstName,
                    Email = adminUserVm.Email,
                    UserName = adminUserVm.UserName
                };
                await _userManager.CreateAsync(adminUser, adminUserVm.Password);
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
            
            if(moderatorUserVm is not null)
            {
                AppUser moderatorUser = new()
                {
                    FirstName = moderatorUserVm.FirstName,
                    Email = moderatorUserVm.Email,
                    UserName = moderatorUserVm.UserName
                };
                await _userManager.CreateAsync(moderatorUser, moderatorUserVm.Password);
                await _userManager.AddToRoleAsync(moderatorUser, "Moderator");
            }


            return Ok("Successfully");
        }
        public async Task SendConfirmEmailAsync(AppUser user)
        {
           var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            // await _userManager.ConfirmEmailAsync(user, token);
   
            string url = Url.Action("ConfirmEmail","Account",new { token, userId = user.Id}, Request.Scheme)?? string.Empty;
            await _emailService.SendEmailAsync(user.Email, "Confirm your email", url);
        }
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
           var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest();
         var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                return BadRequest();

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }

    }
}
