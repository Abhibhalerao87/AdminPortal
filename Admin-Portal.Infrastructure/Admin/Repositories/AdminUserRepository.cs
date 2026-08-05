using Admin_Portal.Core.Admin.Entities;
using Admin_Portal.Core.Admin.Interfaces;
using Admin_Portal.Infrastructure.Admin.Data;
using Microsoft.EntityFrameworkCore;

namespace Admin_Portal.Infrastructure.Admin.Repositories
{
    public class AdminUserRepository : IAdminUserRepository
    {
        private readonly AdminPortalDbContext _context;

        public AdminUserRepository(AdminPortalDbContext context)
        {
            _context = context;
        }

        public async Task<AdminUser?> GetByIdAsync(Guid id)
        {
            return await _context.AdminUsers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AdminUser?> GetByUsernameAsync(string username)
        {
            return await _context.AdminUsers.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
        }

        public async Task<AdminUser?> GetByEmailAsync(string email)
        {
            return await _context.AdminUsers.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
        }

        public async Task<IEnumerable<AdminUser>> GetAllAsync()
        {
            return await _context.AdminUsers.OrderBy(x => x.CreatedAt).ToListAsync();
        }

        public async Task<AdminUser> CreateAsync(AdminUser adminUser)
        {
            adminUser.CreatedAt = DateTime.UtcNow;
            _context.AdminUsers.Add(adminUser);
            await _context.SaveChangesAsync();
            return adminUser;
        }

        public async Task<AdminUser> UpdateAsync(AdminUser adminUser)
        {
            adminUser.UpdatedAt = DateTime.UtcNow;
            _context.AdminUsers.Update(adminUser);
            await _context.SaveChangesAsync();
            return adminUser;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var adminUser = await GetByIdAsync(id);
            if (adminUser == null)
                return false;

            _context.AdminUsers.Remove(adminUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(string username, string email)
        {
            return await _context.AdminUsers
                .AnyAsync(x => x.Username.ToLower() == username.ToLower() || x.Email.ToLower() == email.ToLower());
        }
    }
}
