using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCProniaTask.Contexts;
using MVCProniaTask.ViewModels.UserViewModels;

namespace MVCProniaTask.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager) : Controller
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
            return Ok("OK");
        }
    }
}
