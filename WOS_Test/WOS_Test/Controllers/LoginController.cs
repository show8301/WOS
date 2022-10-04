using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        private readonly IConfiguration _configuration;

        public LoginController(WOSContext wosContext, IConfiguration configuration)
        {
            _wosContext = wosContext;
            _configuration = configuration;
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
                    new Claim(JwtRegisteredClaimNames.GivenName, value.Username),
                    new Claim("FullName", user.Username)                    
                };

                if (user.Username == "test_admin")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "admin"));
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));

                var jwt = new JwtSecurityToken
                (
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

                var token = new JwtSecurityTokenHandler().WriteToken(jwt);
              
                return token;
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

        [HttpGet("NoAccess")]
        public string NoAccess()
        {
            return "權限不足";
        }
    }
}
