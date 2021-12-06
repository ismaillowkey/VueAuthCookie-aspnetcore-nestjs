using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CookieAuthJwtRefresh.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    var hasher = new PasswordHasher<ApplicationUser>();
        //    modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        UserName = "test@test.com",
        //        NormalizedUserName = "test@test.com".ToUpper(),
        //        Email = "test@test.com",
        //        NormalizedEmail = "test@test.com".ToUpper(),
        //        EmailConfirmed = true,
        //        PasswordHash = hasher.HashPassword(null, "admin"),
        //        SecurityStamp = string.Empty
        //    });
        //}
    }
}
