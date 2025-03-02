using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TechBack_Identity.Models.Entitys;

namespace TechBack_Identity.Helpers
{
    public class AddMyClaims:UserClaimsPrincipalFactory<User>
    {
        public AddMyClaims(UserManager<User> userManager,IOptions<IdentityOptions>option):base(userManager,option) 
        {
            
        }
        protected  override async Task<ClaimsIdentity>GenerateClaimsAsync(User user)
        {
            var Identity=await base.GenerateClaimsAsync(user);
            Identity.AddClaim(new Claim("FullName",$"{user.Name}{user.LastName}"));
        return Identity;
        }
    }
}
