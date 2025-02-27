using System.ComponentModel.DataAnnotations;

namespace TechBack_Identity.Models.Dto
{
    public class TwoFactorLoginDto
    {
        [Required]
        public string Code { get; set; }
        public bool IsPersistent { get; set; }
        public string Provider { get; set; }
    }
}
