using CookieAuthJwtRefresh.Data;
using CookieAuthJwtRefresh.DataContract.Request;
using CookieAuthJwtRefresh.DataContract.Response;
using CookieAuthJwtRefresh.Infrastrucutre;
using CookieAuthJwtRefresh.Models;
using CookieAuthJwtRefresh.Services.EmailSenderService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CookieAuthJwtRefresh.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly JwtTokenCreator _jwtCreator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AuthController(ILogger<AuthController> logger, 
            JwtTokenCreator jwtCreator, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _logger = logger;
            _jwtCreator = jwtCreator;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistration obj)
        {
            if (!ModelState.IsValid || obj == null)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed" });
            }

            if (obj.Password != obj.Password_Confirm)
            {
                return new BadRequestObjectResult(new { Message = "User Registration Failed" });
            }

            //var UserExist = await _userManager.FindByNameAsync(obj.Email);
            //if (UserExist != null)
            //{
            //    return new BadRequestObjectResult(new { Message = "Username already Exist", Errors = "Username already Exist" });
            //}

            var EmailExist = await _userManager.FindByEmailAsync(obj.Email);
            if (EmailExist != null)
            {
                return new BadRequestObjectResult(new { Message = "Email already Exist", Errors = "Email already Exist" });
            }

            // username = email
            var identityUser = new ApplicationUser() { First_Name = obj.First_Name, Last_Name = obj.Last_Name, UserName = obj.Email, Email = obj.Email, RefreshToken = Guid.NewGuid().ToString() };
            var result = await _userManager.CreateAsync(identityUser, obj.Password);
            if (!result.Succeeded)
            {
                var dictionary = new ModelStateDictionary();
                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }

                return new BadRequestObjectResult(new { Message = "User Registration Failed", Errors = dictionary });
            }
            var data = new UserInfo
            {
                first_name = obj.First_Name,
                last_name = obj.Last_Name,
                email = obj.Email,
            };
            return StatusCode(StatusCodes.Status201Created, new { statusCode = 201, Message = "success create account", data = data });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginApi([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return BadRequest(new { success = false, message = "Email not found" });
                }
                // username = email
                var signIn = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
                if (signIn.Succeeded)
                {
                    var token = _jwtCreator.Generate(user.Email, user.Id);
                    user.RefreshToken = Guid.NewGuid().ToString();
                    await _userManager.UpdateAsync(user);

                    Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                    Response.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                    Response.Cookies.Append("X-Refresh-Token", user.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

                    var userInfo = new UserInfo()
                    {
                        first_name = user.First_Name,
                        last_name = user.Last_Name,
                        email = user.Email,
                    };
                    return Ok(new { statusCode = 200, message= "User has logged in", data = userInfo });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Invalid credentials" });
                }
            }
            else
                return BadRequest(ModelState);
        }


        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            if (!(Request.Cookies.TryGetValue("X-Username", out var userName) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)))
                return BadRequest(new { statusCode = 401, message = "Unauthorized" });
            var user = await _userManager.Users.FirstOrDefaultAsync(i => i.UserName == userName && i.RefreshToken == refreshToken);
            if (user == null)
                return BadRequest(new { statusCode = 401, message = "Unauthorized" });

            var userInfo = new UserInfo()
            {
                first_name = user.First_Name,
                last_name = user.Last_Name,
                email = user.Email
            };
            return Ok(new { statusCode = 200, message = "", data = userInfo });
        }

        [AllowAnonymous]
        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return BadRequest(new { message = "User Not Found" });

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var EncodeToken = Uri.EscapeDataString(token);
                var url = $"http://localhost:8080/reset/{EncodeToken}/{model.Email}";
                var message = new MessageEmail()
                {
                    To = "ismaillowkey@gmail.com",
                    Subject = "Forgot Password",
                    BodyHtml = $"Click <a href='{url}'> here </a> to reset your password!"
                };
                _emailSender.SendEmail(message);
                return Ok(new { success = true, message = "Check your email" });
            }
            else
                return BadRequest(ModelState);
        }

        [AllowAnonymous]
        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.Password_confirm)
                    return BadRequest(new { message = "User Not Found" });
                var user = await _userManager.FindByEmailAsync(model.Email);
                // validate if token expired
                // ....
                var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (resetPassResult.Succeeded)
                    return Ok(new { success = true, message = "Password has been reset" });
                else
                {
                    var error = resetPassResult.Errors.ToList()[0].Description;
                    return BadRequest(new { success = false, message = "Failed to reset password", error = error });
                }
            }
            else
                return BadRequest(ModelState);
        }


        [AllowAnonymous]
        [HttpGet("refreshToken")]
        public async Task<IActionResult> Refresh()
        {
            if (!(Request.Cookies.TryGetValue("X-Username", out var userName) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)))
                return BadRequest();

            var user = _userManager.Users.FirstOrDefault(i => i.UserName == userName && i.RefreshToken == refreshToken);
            if (user == null)
                return BadRequest();

            var token = _jwtCreator.Generate(user.Email, user.Id);

            user.RefreshToken = Guid.NewGuid().ToString();

            await _userManager.UpdateAsync(user);

            if (HttpContext.Request.Cookies.ContainsKey("X-Access-Token"))
                Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            if (HttpContext.Request.Cookies.ContainsKey("X-Username"))
                Response.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            if (HttpContext.Request.Cookies.ContainsKey("X-Refresh-Token"))
                Response.Cookies.Append("X-Refresh-Token", user.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok();
        }

        
        [AllowAnonymous]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (HttpContext.Request.Cookies.ContainsKey("X-Access-Token"))
                Response.Cookies.Delete("X-Access-Token");
            if (HttpContext.Request.Cookies.ContainsKey("X-Username"))
                Response.Cookies.Delete("X-Username");
            if (HttpContext.Request.Cookies.ContainsKey("X-Refresh-Token"))
                Response.Cookies.Delete("X-Refresh-Token");

            return Ok(new { Message = "You are logged out" });
        }
    }
}
