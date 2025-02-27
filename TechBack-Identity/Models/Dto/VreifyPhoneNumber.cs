using System.ComponentModel.DataAnnotations;

namespace TechBack_Identity.Models.Dto
{
    public class VreifyPhoneNumber
    {
        public string phoneNumber { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
