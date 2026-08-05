using Admin_Portal.Contracts.Admin.Requests;
using Admin_Portal.Contracts.Admin.Responses;

namespace Admin_Portal.Application.Admin.Interfaces
{
    public interface IAdminAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
