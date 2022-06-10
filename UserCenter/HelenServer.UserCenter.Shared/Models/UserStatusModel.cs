namespace HelenServer.UserCenter.Shared
{
    public class UserStatusModel
    {
        public string UserId { get; set; } = string.Empty;

        public UserStatus Status { get; set; }
    }
}