using System.ComponentModel.DataAnnotations;

namespace CookieAuthJwtRefresh.DataContract.Request
{
    public class UserRegistration
    {
        [Required]
        public string First_Name { get; set; }
        [Required]
        public string Last_Name { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Password_Confirm { get; set; }
    }
}
