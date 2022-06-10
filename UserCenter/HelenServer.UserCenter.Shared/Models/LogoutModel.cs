namespace HelenServer.UserCenter.Shared
{
    public class LogoutModel
    {
        public string LogoutId { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public string LogoutIframeUrl { get; set; }
        public bool ShowLogoutPrompt { get; set; }
        public string ExternalAuthenticationScheme { get; set; }
    }
}
