using System.ComponentModel.DataAnnotations;

namespace TechBack_Identity.Models.Dto
{
    public class SetphoneNumberDto
    {
        [Required]
        public string PhoneNumber { get; set; }
    }
}
