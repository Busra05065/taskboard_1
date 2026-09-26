namespace TaskBoard.Web.Models
{
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UserInfoResponse
    {
        public bool IsAuthenticated { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
    }
}