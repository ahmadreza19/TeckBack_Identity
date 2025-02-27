using System.Reflection.Metadata.Ecma335;

namespace TechBack_Identity.Models.Dto
{
    public class MyAccountInfoDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string Email { get; set; }
        public bool EmailConfrimed { get; set; }
        public string User { get; set; }
        public string Id { get; set; }
        public bool TwoFactorEnabled { get; set; }

    }
}
