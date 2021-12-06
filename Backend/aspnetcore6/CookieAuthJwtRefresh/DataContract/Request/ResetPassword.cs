namespace CookieAuthJwtRefresh.DataContract.Request
{
    public class ResetPassword
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string Password_confirm { get; set; }
    }
}
