using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechBack_Identity.Areas.Admin.Models;
using TechBack_Identity.Models.Entitys;
using TechBack_Identity.Models.Register;

namespace TechBack_Identity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult UserList()
        {
            var user = _userManager.Users.Select(p => new UserListDto
            {
                Id = p.Id,
                FrestName = p.Name,
                LastName = p.LastName,
                EmailConfirmed = p.EmailConfirmed,
                PhoneNumber = p.PhoneNumber,
                UserName = p.UserName,
                AccessFailedCount = p.AccessFailedCount
            }).ToList();
            return View(user);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Register register)
        {
            User user = new User()
            {
                Email = register.Email,
                LastName = register.LastName,
                Name = register.FerstName,
                UserName = register.Email,
            };
            var resulte = _userManager.CreateAsync(user, register.Password).Result;
            if (resulte.Succeeded == true)
            {
                return Redirect("Userlist");
            }
            string message = "";
            foreach (var item in resulte.Errors.ToList())
            {
                message += item.Description += Environment.NewLine;
            }
            TempData["Message"] = message;
            return View(resulte);
        }


        public IActionResult EditUser(string Id)
        {
            var user = _userManager.FindByIdAsync(Id).Result;

            UserEditDto userEdit = new UserEditDto()
            {
                Email = user.Email,
                FirstName = user.Name,
                Id = user.Id,
                LastName = user.LastName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
            };
            return View(userEdit);
        }
        [HttpPost]
        public IActionResult EditUser(UserEditDto userEdit)
        {
            var user = _userManager.FindByIdAsync(userEdit.Id).Result;

            user.Name = userEdit.FirstName;
            user.LastName = userEdit.LastName;
            user.PhoneNumber = userEdit.PhoneNumber;
            user.Email = userEdit.Email;
            user.UserName = userEdit.UserName;

            var resulte = _userManager.UpdateAsync(user).Result;
            if (resulte.Succeeded)
            {
                return RedirectToAction("UserList", "User", new { area = "Admin" });
            }
            string message = "";
            foreach (var item in resulte.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;

            return View(userEdit);
        }

        public IActionResult Delete(string Id)
        {
         var user=_userManager.FindByIdAsync(Id).Result;
            UserDeleteDto userDelete = new UserDeleteDto()
            {
               FirstName=user.Name,
               LastName=user.LastName,
               UserName=user.UserName,
               Id =user.Id,
               Email=user.Email,
            };
            return View(userDelete);
        }
        [HttpPost]
        public IActionResult Delete(UserDeleteDto userDelete)
        {
           var user = _userManager.FindByIdAsync(userDelete.Id).Result;
            var resulte=_userManager.DeleteAsync(user).Result;
            if (resulte.Succeeded) 
            {
                return RedirectToAction("UserList", "user",new  {Areas="User" });
            }
            string message = "";
            foreach (var item in resulte.Errors.ToList())
            {
                  message+=item.Description + Environment.NewLine;
            }
            return View(userDelete);
        }

    }

}
