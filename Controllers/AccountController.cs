using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCProniaTask.Contexts;
using MVCProniaTask.ViewModels.UserViewModels;

namespace MVCProniaTask.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager) : Controller
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

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
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
            AppUser adminUser = new()
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
            };

            await _userManager.CreateAsync(adminUser, "Admin123!");
            await _userManager.CreateAsync(moderatorUser, "Moderator123!");

            await _userManager.AddToRoleAsync(adminUser, "Admin");
            await _userManager.AddToRoleAsync(moderatorUser, "Moderator");

            return Ok("Successfully");
        }

    }
}
