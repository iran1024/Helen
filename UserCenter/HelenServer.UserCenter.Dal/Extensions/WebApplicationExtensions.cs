using HelenServer.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HelenServer.UserCenter.Dal
{
    public static class WebApplicationExtensions
    {
        public static WebApplication ApplyDataInitialize(this WebApplication app)
        {
            var context = app.Services.GetRequiredService<UserCenterDbContext>();

            context.InitPersistedModel();

            return app;
        }
    }
}