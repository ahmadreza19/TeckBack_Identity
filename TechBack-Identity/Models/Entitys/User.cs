using Microsoft.AspNetCore.Identity;

namespace TechBack_Identity.Models.Entitys
{
    public class User:IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
