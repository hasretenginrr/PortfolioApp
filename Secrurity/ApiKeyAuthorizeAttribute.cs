using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace PortfolioBackend.Security
{
    // Bu attribute, bir controller'ın veya metodun üstüne [ApiKeyAuthorize] olarak eklenecek.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "X-Api-Key"; // Key'in header'da hangi isimle geleceği

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1. appsettings.json'dan bizim gizli key'imizi oku
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var secretApiKey = configuration.GetValue<string>("AdminSecretKey");

            // 2. Kullanıcının isteğinin (request) header'ından gelen key'i oku
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
            {
                // Header'da key yoksa, yetkisiz (Unauthorized) hatası ver
                context.Result = new UnauthorizedObjectResult("API Key eksik.");
                return;
            }

            // 3. İki key'i karşılaştır
            if (!secretApiKey.Equals(potentialApiKey))
            {
                // Key'ler eşleşmiyorsa, yetkisiz (Unauthorized) hatası ver
                context.Result = new UnauthorizedObjectResult("Geçersiz API Key.");
                return;
            }

            // 4. Key doğruysa, isteğin devam etmesine izin ver
            await next();
        }
    }
}