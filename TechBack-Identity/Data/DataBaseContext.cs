using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechBack_Identity.Models.Entitys;

namespace TechBack_Identity.Data
{
    public class DataBaseContext : IdentityDbContext<User,Role,string>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }
      
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<IdentityUserLogin<string>>().HasKey(p=>new{p.ProviderKey,p.LoginProvider });
        //    builder.Entity<IdentityUserRole<string>>().HasKey(p=>new{p.UserId,p.RoleId });
        //    builder.Entity<IdentityUserToken<string>>().HasKey(p=>new{p.UserId,p.LoginProvider });
        //}
    }
}
