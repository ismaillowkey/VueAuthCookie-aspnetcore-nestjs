using System.ComponentModel.DataAnnotations;

namespace CookieAuthJwtRefresh.DataContract.Request
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
