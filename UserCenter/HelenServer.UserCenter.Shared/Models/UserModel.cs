namespace HelenServer.UserCenter.Shared
{
    public class UserModel
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? Name { get; set; }
        public string Sex { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string[] Roles { get; set; } = Array.Empty<string>();
        public int? JobNumer { get; set; }
        public DateTime? InductionDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? LastIp { get; set; }
        public DateTime? LastLoginTime { get; set; }
    }
}