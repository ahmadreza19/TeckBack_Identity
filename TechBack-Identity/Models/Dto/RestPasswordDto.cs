using System.ComponentModel.DataAnnotations;

namespace TechBack_Identity.Models.Dto
{
    public class RestPasswordDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }

    }
}
