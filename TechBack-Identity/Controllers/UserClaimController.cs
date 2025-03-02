using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechBack_Identity.Models.Entitys;

namespace TechBack_Identity.Controllers
{
    public class UserClaimController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UserClaimController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View(User.Claims);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string ClaimValue, string ClaimType)
        {
            var user = _userManager.GetUserAsync(User).Result;

            Claim newclaim = new Claim(ClaimType, ClaimValue, ClaimValueTypes.String);

            var result = _userManager.AddClaimAsync(user,newclaim).Result;
            if (result.Succeeded)
            {
                return Redirect("Index");
            }
            else
            {
                foreach (var claim in result.Errors)
                {
                    ModelState.AddModelError("", claim.Description);
                }
            }
            return View();
        }
        public IActionResult Delete(string ClaimType)
        {
           var user=_userManager.GetUserAsync(User).Result;
            Claim claim=User.Claims.Where(p=>p.Type==ClaimType).FirstOrDefault();
            if (claim != null)
            {
                var resulte = _userManager.RemoveClaimAsync(user, claim).Result;
                if (resulte.Succeeded)
                {
                    return Redirect("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
