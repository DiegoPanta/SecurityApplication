using Presentation.Model.Security;

namespace Presentation.Interfaces
{
    public interface IAuthService
    {
        Task<string?> RequestLoginAsync(string email);
        string GenerateRandomCode();
    }
}
