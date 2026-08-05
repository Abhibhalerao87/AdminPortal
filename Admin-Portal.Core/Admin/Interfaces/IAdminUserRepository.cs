using Admin_Portal.Core.Admin.Entities;

namespace Admin_Portal.Core.Admin.Interfaces
{
    public interface IAdminUserRepository
    {
        Task<AdminUser?> GetByIdAsync(Guid id);
        Task<AdminUser?> GetByUsernameAsync(string username);
        Task<AdminUser?> GetByEmailAsync(string email);
        Task<IEnumerable<AdminUser>> GetAllAsync();
        Task<AdminUser> CreateAsync(AdminUser adminUser);
        Task<AdminUser> UpdateAsync(AdminUser adminUser);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(string username, string email);
    }
}
