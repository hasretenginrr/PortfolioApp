using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace PortfolioBackend.Pages
{
    public class AdminLogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
           
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

           
            return RedirectToPage("/Index"); 
        }
    }
}