using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace PortfolioBackend.Pages
{
    public class AdminLoginModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public AdminLoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        [Required(ErrorMessage = "Parola gereklidir.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

           
            string correctPassword = _configuration.GetValue<string>("AdminSettings:Password");

            
            if (Password == correctPassword)
            {
                
                var claims = new List<Claim>
                {
                   
                    new Claim(ClaimTypes.Name, "Admin"),
                    new Claim(ClaimTypes.Role, "AdminUser")
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                   
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
                };

               
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

               
                return LocalRedirect(returnUrl ?? "/Admin/Index");
            }
            else
            {
              
                ModelState.AddModelError(string.Empty, "Geçersiz parola.");
                return Page();
            }
        }
    }
}