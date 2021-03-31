using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ServerSideAuthenticationExample.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServerSideAuthenticationExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("hello")]
        public async Task<IActionResult> hello()
        {


            return Ok(new
            {
                success = true,
                message = "hello"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SessionRequest login)
        {
            string message = "";
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, login.session_id),
                    new Claim(ClaimTypes.Name, login.session_id),
                    new Claim(ClaimTypes.Role, "user"),
                };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = login.keep_login, //쿠키를 브라우저를 닫아도 일정기간동안엔 유지되도록 함 (자동로그인 기능)
                RedirectUri = this.Request.Host.Value,
            };

            try
            {
                await HttpContext.SignInAsync(
                    "Cookies",
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                message = "성공";
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            IActionResult response = Ok(new { result = true, message = message });
            return response;
        }
    }
}
