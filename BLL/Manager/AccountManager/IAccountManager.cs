using Presentation.DTOs.Responses;
using System.Threading.Tasks;

namespace BLL.Manager.AccountManager
{
    public interface IAccountManager
    {
        Task<AuthResponse?> GetFullUserResponseAsync(string userId, string role, string token, int expiryMinutes);
        Task<string?> UpdateProfileAsync(string userId, string role, Presentation.DTOs.Requests.UpdateProfileRequest request);
        Task<string?> ChangePasswordAsync(string userId, Presentation.DTOs.Requests.ChangePasswordRequest request);
    }
}
