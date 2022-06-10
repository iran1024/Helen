namespace HelenServer.Core
{
    public class OperationUser
    {
        protected OperationUser(OperationUser user)
            : this(user.UserId, user.Username, user.Roles, user.IsAuthenticated)
        {

        }

        public OperationUser(string userId, string userName, IEnumerable<string> roles, bool isAuthenticated)
        {
            UserId = userId;

            Username = userName;

            Roles = roles.ToArray();

            IsAuthenticated = isAuthenticated;
        }

        public static OperationUser None
            => new(string.Empty, string.Empty, Array.Empty<string>(), false);

        public bool IsAuthenticated { get; }

        public string UserId { get; }

        public string Username { get; }

        public string[] Roles { get; }
    }
}