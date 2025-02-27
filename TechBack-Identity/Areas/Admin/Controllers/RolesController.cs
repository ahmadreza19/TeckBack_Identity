using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechBack_Identity.Areas.Admin.Models.Roles;
using TechBack_Identity.Models.Entitys;

namespace TechBack_Identity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        public RolesController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
          var user=  _roleManager.Roles
                .Select(p => new RolesDto
                {
                    Description = p.Description,
                    Id = p.Id,
                    Name = p.Name
                }).ToList();
            return View(user);
        }
        [HttpGet]
        public IActionResult create()
        {
            return View();
        }
        public IActionResult create(AddNewRolesDto rolesDto)
        {
            Role role = new Role()
            {
                Name = rolesDto.Name,
                Description = rolesDto.Description,
            };
            var user = _roleManager.CreateAsync(role).Result;
            if (user.Succeeded)
            {
                return RedirectToAction("Index", "Roles", new { Areas = "Admin" });
            }
          ViewBag.Error=  user.Errors.ToList();
            return View(rolesDto);
        }

    }
}
