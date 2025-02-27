namespace TechBack_Identity.Areas.Admin.Models
{
    public class UserListDto
    {
        public string Id { get; set; }
        public string LastName { get; set; }
        public string FrestName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public int  AccessFailedCount { get; set; }

    }
}
