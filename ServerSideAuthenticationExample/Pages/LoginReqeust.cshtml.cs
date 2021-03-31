using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

//추가
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text.Json;
using ServerSideAuthenticationExample.Data;

namespace ServerSideAuthenticationExample.Pages
{
    public class LoginReqeustModel : PageModel
    {
        public void OnGet()
        {
        }

        /*public async Task<IActionResult> OnPostAsync([FromBody] SessionRequest request)
        {
            return this.Content(JsonSerializer.Serialize(new
            {
                result = true,
            }));
        }*/


        public async Task<IActionResult> OnPostAsync(string session_id, bool keep_login)
        {
            string returnUrl = Url.Content("~/");
            bool result = true;
            string message = "로그인 처리됬습니다";
            var code = "";
            var key = "";
            try
            {
                try
                {
                    // Clear the existing external cookie
                    await HttpContext
                        .SignOutAsync(
                        "Cookies");
                }
                catch
                {
                }

                // *** !!! This is where you would validate the user !!! ***
                // In this example we just log the user in
                // (Always log the user in for this demo)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, session_id),
                    new Claim(ClaimTypes.Name, session_id),
                    new Claim(ClaimTypes.Role, "user"),
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = keep_login, //쿠키를 브라우저를 닫아도 일정기간동안엔 유지되도록 함 (자동로그인 기능)
                    RedirectUri = this.Request.Host.Value,
                };

                try
                {
                    await HttpContext.SignInAsync(
                        "Cookies",
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }


            return this.Content(JsonSerializer.Serialize(new
            {
                result = result,
                code = code,
                message = message,
                key = key
            }));
/*
            return this.Content(JObject.FromObject(new
            {
                result = result,
                code = code,
                message = message,
                key = key
            }).ToString());*/
        }
    }
}
