using System.Threading.Tasks;

namespace PortfolioBackend.Services
{
    public interface IEmailService
    {
        Task SendContactEmail(string fromName, string fromEmail, string subject, string messageBody);
    }
}
