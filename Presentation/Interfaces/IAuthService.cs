using Presentation.Model.Security;

namespace Presentation.Interfaces
{
    public interface IAuthService
    {
        Task<string?> RequestLoginAsync(string email);
        bool VerifyLogin(UserConfiguration user);
        bool VerifyLogin(string token);
    }
}
