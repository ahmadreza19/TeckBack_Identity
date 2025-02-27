using Microsoft.AspNetCore.Identity;

namespace TechBack_Identity.Models.Entitys
{
    public class Role:IdentityRole
    {
        public string Description { get; set; }
        
    }
}
