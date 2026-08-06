using Admin_Portal.Contracts.Admin.Requests;
using Admin_Portal.Contracts.Admin.Responses;
using Admin_Portal.Core.Admin.Entities;
using Admin_Portal.Core.Admin.Enums;
using Admin_Portal.Core.Admin.Interfaces;
using Admin_Portal.Application.Admin.Interfaces;
using Admin_Portal.Shared.Utilities;

namespace Admin_Portal.Application.Admin.Services
{
    public class AdminAuthService : IAdminAuthService
    {
        private readonly IAdminUserRepository _adminUserRepository;

        public AdminAuthService(IAdminUserRepository adminUserRepository)
        {
            _adminUserRepository = adminUserRepository;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var response = new RegisterResponse();

            // Validate input
            var validationErrors = ValidateRegisterRequest(request);
            if (validationErrors.Count > 0)
            {
                response.Success = false;
                response.Message = "Validation failed";
                response.Errors = validationErrors;
                return response;  
            }

            // Check if user already exists
            if (await _adminUserRepository.ExistsAsync(request.Username, request.Email))
            {
                response.Success = false;
                response.Message = "Username or Email already exists";
                response.Errors = new[] { "Username or Email is already registered" };
                return response;
            }

            // Create new admin user
            var adminUser = new AdminUser
            {
                Id = Guid.NewGuid(),
                Username = request.Username.Trim(),
                Email = request.Email.Trim().ToLower(),
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                Role = (UserRole)request.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                var createdUser = await _adminUserRepository.CreateAsync(adminUser);
                response.Success = true;
                response.Message = "Admin registered successfully";
                response.User = MapToAdminUserDto(createdUser);
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred during registration";
                response.Errors = new[] { ex.Message };
                return response;
            }
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var response = new LoginResponse();

            // Validate input
            var validationErrors = ValidateLoginRequest(request);
            if (validationErrors.Count > 0)
            {
                response.Success = false;
                response.Message = "Validation failed";
                response.Errors = validationErrors;
                return response;
            }

            // Get user by username
            var adminUser = await _adminUserRepository.GetByUsernameAsync(request.Username);
            if (adminUser == null)
            {
                response.Success = false;
                response.Message = "Invalid username or password";
                response.Errors = new[] { "User not found" };
                return response;
            }

            // Verify password
            if (!PasswordHasher.VerifyPassword(request.Password, adminUser.PasswordHash))
            {
                response.Success = false;
                response.Message = "Invalid username or password";
                response.Errors = new[] { "Password is incorrect" };
                return response;
            }

            // Check if user is active
            if (!adminUser.IsActive)
            {
                response.Success = false;
                response.Message = "User account is inactive";
                response.Errors = new[] { "Your account has been disabled" };
                return response;
            }

            // Update last login time
            adminUser.LastLoginAt = DateTime.UtcNow;
            await _adminUserRepository.UpdateAsync(adminUser);

            response.Success = true;
            response.Message = "Login successful";
            response.User = MapToAdminUserDto(adminUser);
            return response;
        }

        public async Task<IEnumerable<AdminUser>> GetAllUsers()
        {
            var adminUser = await _adminUserRepository.GetAllAsync();
            return adminUser;
        }

        private List<string> ValidateRegisterRequest(RegisterRequest request)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(request.Username) || request.Username.Length < 3)
                errors.Add("Username must be at least 3 characters long");

            if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
                errors.Add("Email is invalid");

            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
                errors.Add("Password must be at least 6 characters long");

            if (!Enum.IsDefined(typeof(UserRole), request.Role))
                errors.Add("Invalid role specified");

            return errors;
        }

        private List<string> ValidateLoginRequest(LoginRequest request)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(request.Username))
                errors.Add("Username is required");

            if (string.IsNullOrWhiteSpace(request.Password))
                errors.Add("Password is required");

            return errors;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private AdminUserDto MapToAdminUserDto(AdminUser adminUser)
        {
            return new AdminUserDto
            {
                Id = adminUser.Id,
                Username = adminUser.Username,
                Email = adminUser.Email,
                Role = adminUser.Role.ToString(),
                IsActive = adminUser.IsActive,
                CreatedAt = adminUser.CreatedAt,
                UpdatedAt = adminUser.UpdatedAt,
                LastLoginAt = adminUser.LastLoginAt
            };
        }
    }
}
