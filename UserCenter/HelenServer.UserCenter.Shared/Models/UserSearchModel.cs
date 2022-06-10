namespace HelenServer.UserCenter.Shared
{
    public class UserSearchModel
    {
        public string? Keyword { get; set; }
        public UserStatus Status { get; set; }
        public bool IsDeleted { get; set; }
    }
}