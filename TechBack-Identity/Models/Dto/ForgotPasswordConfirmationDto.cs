using System.ComponentModel.DataAnnotations;

namespace TechBack_Identity.Models.Dto
{
    public class ForgotPasswordConfirmationDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
