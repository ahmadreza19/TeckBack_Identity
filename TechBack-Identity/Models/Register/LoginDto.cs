using System.ComponentModel.DataAnnotations;

namespace TechBack_Identity.Models.Register
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersistent { get; set; } = false;
        public string ReturnUrl { get; set; }
    }
}
