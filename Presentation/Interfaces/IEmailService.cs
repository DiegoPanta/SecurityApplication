namespace Presentation.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string email, string token);
    }
}
