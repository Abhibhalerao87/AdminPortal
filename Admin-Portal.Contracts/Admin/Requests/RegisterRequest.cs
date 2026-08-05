namespace Admin_Portal.Contracts.Admin.Requests
{
    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int Role { get; set; } = 4; // Default to User role
    }
}
