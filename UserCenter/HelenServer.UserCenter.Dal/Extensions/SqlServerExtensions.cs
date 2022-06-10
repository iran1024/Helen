using HelenServer.Core;
using HelenServer.Data.SqlServer;
using HelenServer.Grpc.Contracts;
using HelenServer.UserCenter.Shared;
using GrpcCore = Grpc.Core;

namespace HelenServer.UserCenter.Dal
{
    public static class SqlServerExtensions
    {
        private static UserCenterDbContext _context = null!;
        private static IGrpcClientFactory _factory = null!;

        public static void Config(Action<SqlServerExtensionsOptionsBuilder> optionsAction)
        {
            var builder = new SqlServerExtensionsOptionsBuilder();

            optionsAction(builder);

            _context = builder.Options.DbContext as UserCenterDbContext ?? throw new ArgumentNullException("Context", "Context必须初始化");
            _factory = builder.Options.GrpcClientFactory ?? throw new ArgumentNullException("gRPC Client Factoy", "工程必须初始化");
        }

        public static UserModel ToModel(this User entity)
        {
            return new UserModel()
            {
                UserId = entity.Id,
                Username = entity.Username,
                Avatar = GetAvatar(entity.Avatar),
                Name = entity.Name,
                Sex = entity.Sex == 1 ? "男" : "女",
                Department = _context.GetDepartment(entity.Department, null)?.Name ?? string.Empty,
                Position = _context.GetPosition(entity.Position, null)?.Name ?? string.Empty,
                Roles = entity.Roles is not null ? Helen.ResolveCompositField(entity.Roles) : Array.Empty<string>(),
                JobNumer = entity.JobNumer,
                InductionDate = entity.InductionDate,
                Status = Helen.GetEnumName<UserStatus>(entity.Status),
                Email = entity.Email,
                LastIp = entity.LastIp,
                LastLoginTime = entity.LastLoginTime
            };
        }

        public static User ToEntity(this UserModel userModel)
        {
            return new User()
            {
                Id = userModel.UserId,
                Username = userModel.Username,
                Avatar = userModel.Avatar,
                Name = userModel.Name,
                Sex = userModel.Sex == "男" ? 1 : 0,
                Department = _context.GetDepartment(null, userModel.Department)?.Id,
                Position = _context.GetPosition(null, userModel.Position)?.Id,
                Roles = userModel.Roles is not null && userModel.Roles.Length > 0 ? Helen.CompositField(userModel.Roles) : string.Empty,
                JobNumer = userModel.JobNumer,
                InductionDate = userModel.InductionDate,
                Status = (int)userModel.Status.ToEnum<UserStatus>(),
                Email = userModel.Email,
                LastIp = userModel.LastIp,
                LastLoginTime = userModel.LastLoginTime
            };
        }

        private static string GetAvatar(string? netpath)
        {
            if (string.IsNullOrEmpty(netpath))
            {
                return string.Empty;
            }
            else
            {
                var result = _factory.GetService<IASGetFile, string, GrpcResult<Stream>>("as").InvokeAsync(netpath, new GrpcCore.Metadata(), CancellationToken.None).Result;

                var suffix = Path.GetExtension(netpath);

                var base64 = Helen.Base64EncodeImage(result.Data, suffix);

                return base64;
            }
        }
    }
}