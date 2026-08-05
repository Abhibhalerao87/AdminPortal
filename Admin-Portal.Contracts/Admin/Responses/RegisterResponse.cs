namespace Admin_Portal.Contracts.Admin.Responses
{
    public class RegisterResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public AdminUserDto? User { get; set; }

        public IEnumerable<string> Errors { get; set; } = new List<string>();
    }
}
