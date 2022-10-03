using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WOS_Test.Dtos;
using WOS_Test.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WOS_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly WOSContext _wosContext;

        public LoginController(WOSContext wosContext)
        {
            _wosContext = wosContext;
        }
        
        [HttpPost]
        public string Login(LoginPostDto value)
        {
            var user = (from a in _wosContext.UserData
                        where a.Username == value.Username
                        && a.Password == value.Password
                        select a).SingleOrDefault();

            if(user == null)
            {
                return "帳號或密碼錯誤";
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, value.Username),
                    new Claim("FullName", user.Username),

                };
                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

                return "登入成功";
            }
        }

        [HttpDelete]
        public void logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet("NoLogin")]
        public string NoLogin()
        {
            return "未登入";
        }
    }
}
