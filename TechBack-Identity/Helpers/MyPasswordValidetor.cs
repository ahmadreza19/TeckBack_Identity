using Microsoft.AspNetCore.Identity;
using TechBack_Identity.Models.Entitys;

namespace TechBack_Identity.Helpers
{
    public class MyPasswordValidetor : IPasswordValidator<User>
    {
        IList<string> CommanPassword = new List<string>()
        {
            "123456" ,"Ahmad@1315"
        };
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string? password)
        {
            if (CommanPassword.Contains(password))
            {
                var resute = IdentityResult.Failed(new IdentityError
                {
                
                Code= "CommanPassword",
                Description="یک پسورد قوی انتخاب کنید این پسورد توسط ربات های هکر هک میشود!"
                });
                return Task.FromResult(resute);
            }
            return Task.FromResult(IdentityResult.Success);
        }

    }
}
