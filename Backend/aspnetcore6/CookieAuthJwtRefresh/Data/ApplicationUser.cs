using Microsoft.AspNetCore.Identity;

namespace CookieAuthJwtRefresh.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string RefreshToken { get; set; }
        //public List<RefreshToken> RefreshTokens { get; set; }
    }
}
