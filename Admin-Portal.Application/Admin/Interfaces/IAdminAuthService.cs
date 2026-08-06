using Admin_Portal.Contracts.Admin.Requests;
using Admin_Portal.Contracts.Admin.Responses;
using Admin_Portal.Core.Admin.Entities;

namespace Admin_Portal.Application.Admin.Interfaces
{
    public interface IAdminAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<IEnumerable<AdminUser>> GetAllUsers();
    }
}
