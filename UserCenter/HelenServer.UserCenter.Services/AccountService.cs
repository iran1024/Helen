using HelenServer.Core;
using HelenServer.Grpc.Contracts;
using HelenServer.UserCenter.Contracts;
using HelenServer.UserCenter.Shared;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HelenServer.UserCenter.Services
{
    [Injection(typeof(IAccountService))]
    public class AccountService : IAccountService
    {
        private readonly IDalUserService _service;
        private readonly IGrpcClientFactory _factory;
        private readonly IAdvancedDistributedCache _cache;
        private readonly IConfiguration _config;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IDalUserService service,
            IGrpcClientFactory factory,
            IAdvancedDistributedCache cache,
            IConfiguration config,
            ILogger<AccountService> logger)
        {
            _service = service;
            _factory = factory;
            _cache = cache;
            _config = config;
            _logger = logger;
        }

        public async Task<OperationResult> DeleteAsync(Operation<string> operation, CancellationToken cancellationToken)
        {
            if (!await _service.ExistsAsync(operation.Parameter, cancellationToken))
            {
                return OperationResult.Failed("用户不存在");
            }

            if (await _service.DeleteAsync(operation, cancellationToken))
            {
                return OperationResult.OK();
            }
            else
            {
                return OperationResult.Failed("删除失败");
            }
        }

        public Task<bool> ExistAsync(RegisterModel registerModel, CancellationToken cancellationToken)
        {
            return _service.ExistUserAsync(registerModel, cancellationToken);
        }

        public async Task<OperationResult> ExistsAccountAsync(string account, CancellationToken cancellationToken)
        {
            if (await _service.ExistsAsync(account, cancellationToken))
            {
                return OperationResult.OK("账号已存在");
            }

            return OperationResult.Failed("账号不存在");
        }

        public async Task<OperationResult<UserModel>> GetAsync(OperationUser operation, CancellationToken cancellationToken)
        {
            var user = await _service.GetAsync(new Operation<string>(operation)
            {
                Parameter = operation.UserId
            }, cancellationToken);

            if (user is null)
            {
                return OperationResult<UserModel>.Failed("用户不存在");
            }

            return OperationResult<UserModel>.OK(user);
        }

        public Task<IReadOnlyCollection<UserModel>> GetListAsync(Operation<UserSearchModel> operation, CancellationToken cancellationToken)
        {
            return _service.GetListAsync(operation, cancellationToken);
        }

        public async Task<OperationResult> RegistAsync(RegisterModel model, CancellationToken cancellationToken)
        {
            string? verifyCode = await _cache.GetAsync<string>(model.Username, cancellationToken);

            if (string.IsNullOrWhiteSpace(model.VerificationCode) || verifyCode != model.VerificationCode)
            {
                return OperationResult.Failed("验证码错误");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return OperationResult.Failed("密码不一致");
            }

            var userModel = new UserModel
            {
                UserId = Helen.NewGuid,
                Username = model.Username,
                Password = model.Password,
                Status = Helen.GetEnumName<UserStatus>((int)UserStatus.OnDuty)
            };


            if (await _service.ExistsAsync(model.Username, cancellationToken))
            {
                return OperationResult.Failed("邮箱已注册");
            }

            userModel.Email = model.Username;

            var result = await _service.AddAsync(userModel, cancellationToken);

            if (result == 0)
            {
                return OperationResult.Failed("注册失败");
            }

            await _cache.RemoveAsync(model.Username, cancellationToken);

            return OperationResult.OK("注册成功");
        }

        public async Task<OperationResult> SendVerifyCodeAsync(RegisterModel model, CancellationToken cancellationToken)
        {
            var mailConfig = _config.GetSection("Mail");

            var mailServerHost = mailConfig.GetValue("ServerHost", "mail.dev.appeon.net");

            var mailServerPort = mailConfig.GetValue("ServerPort", 10);

            var fromAccount = mailConfig.GetValue("FromAccount", "dotnet@dev.appeon.net");

            var password = mailConfig.GetValue("Password", "VAX5$&sL");

            var expiration = mailConfig.GetValue("Expiration", 10);

            var useSSL = mailConfig.GetValue("UseSSL", true);

            if (string.IsNullOrWhiteSpace(mailServerHost) || mailServerPort == 0 || string.IsNullOrWhiteSpace(fromAccount))
            {
                return OperationResult.Failed("获取配置失败");
            }

            var verifyCode = Helen.GenerateVerifyCode(6);

            await _cache.SetAsync(
                $"{model.Username}",
                verifyCode,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expiration),
                }, cancellationToken);

            return OperationResult.OK();
        }

        public async Task<OperationResult> UpdateAsync(Operation<UserModel> operation, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(operation.Parameter.Username))
            {
                return OperationResult.Failed("用户ID不能为空");
            }

            if (!await _service.ExistsAsync(operation.Parameter.UserId, cancellationToken))
            {
                return OperationResult.Failed("用户不存在");
            }

            var user = await _service.GetAsync(operation.To(s => s.UserId), cancellationToken);

            if (!string.IsNullOrWhiteSpace(operation.Parameter.Avatar))
            {
                if (!operation.Parameter.Avatar.StartsWith("data:image"))
                {
                    return OperationResult<UserModel>.Failed("头像数据格式错误");
                }

                var tclient = _factory.GetService<IASUploadFile, string, GrpcResult<string>>("as");

                var operationUser = new OperationUser(
                    operation.Parameter.UserId,
                    user.Username,
                    operation.Roles,
                    operation.IsAuthenticated);

                var reply = await tclient.InvokeAsync(operation.Parameter.Avatar, operationUser.ToMetadata(), cancellationToken);

                if (reply.IsSuccess)
                {
                    operation.Parameter.Avatar = reply.Data;
                }
                else
                {
                    return OperationResult<UserModel>.Failed(reply.Message);
                }
            }

            if (await _service.UpdateAsync(operation, cancellationToken))
            {
                if (!string.IsNullOrWhiteSpace(user.Avatar))
                {
                    var tclient = _factory.GetService<IASDeleteFile, AttachmentQueryModel, GrpcResult>("as");

                    string[] strArray = user.Avatar.Split("/");

                    var attachmentQuery = new AttachmentQueryModel
                    {
                        AttachmentNo = strArray[^1],
                        ProviderId = strArray[^2],
                    };

                    var reply = await tclient.InvokeAsync(attachmentQuery, operation.ToMetadata(), cancellationToken);
                }

                return OperationResult.OK();
            }
            else
            {
                return OperationResult.Failed("更新失败");
            }
        }

        public async Task<OperationResult> UpdatePasswordAsync(RegisterModel model, CancellationToken cancellationToken)
        {
            var verifyCode = await _cache.GetAsync<string>(model.Username, cancellationToken);

            if (string.IsNullOrWhiteSpace(model.VerificationCode) || verifyCode != model.VerificationCode)
            {
                return OperationResult.Failed("验证码错误");
            }

            if (model.Password == model.ConfirmPassword)
            {
                var operational = new Operation<UserPasswordModel>(OperationUser.None)
                {
                    Parameter = new UserPasswordModel
                    {
                        Username = model.Username,
                        Password = model.Password,
                    }
                };

                if (!await _service.ExistsAsync(model.Username, cancellationToken))
                {
                    return OperationResult.Failed("用户名未注册");
                }

                if (!await _service.UpdatePasswordAsync(operational, cancellationToken))
                {
                    return OperationResult.Failed("密码更新失败");
                }

                await _cache.RemoveAsync(model.Username, cancellationToken);

                return OperationResult.OK("密码更新成功");
            }

            return OperationResult.Failed("密码不一致");
        }

        public async Task<OperationResult> UpdateStatusAsync(Operation<UserStatusModel> operation, CancellationToken cancellationToken)
        {
            if (await _service.UpdateStatusAsync(operation, cancellationToken))
            {
                return OperationResult.OK("用户状态更新成功");
            }

            return OperationResult.Failed("用户状态更新失败");
        }
    }
}