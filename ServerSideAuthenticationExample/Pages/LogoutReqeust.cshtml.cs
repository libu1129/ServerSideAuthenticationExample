using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ServerSideAuthenticationExample.Pages
{
    public class LogoutReqeustModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            bool result = true;
            string message = "logout success";
            try
            {
                Console.WriteLine("Posting logout..");
                await HttpContext.SignOutAsync("Cookies");
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message;
            }

            
            return this.Content(JsonSerializer.Serialize(new
            {
                success = result,
                message = message
            }));
        }
    }
}
