namespace HelenServer.UserCenter.Shared
{
    public class LoginModel
    {
        public string LoginType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? VerificationCode { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}