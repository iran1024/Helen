namespace HelenServer.UserCenter.Shared
{
    public class RegisterBaseModel
    {
        public string Username { get; set; } = string.Empty;

        public OperationType? OperationType { get; set; }
    }

    public class RegisterModel : RegisterBaseModel
    {
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
    }
}