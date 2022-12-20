using WebBriliFresh.Models;
using WebBriliFresh.Models.DTO;

namespace WebBriliFresh.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {

        Task<Status> LoginAsync(LoginModel model);
        Task LogoutAsync();
        Task<Status> RegisterAsync(RegistrationModel model);
        Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username);
        Task<Status> ResetPasswordAsync(ResetPasswordModel model);
    }
}
