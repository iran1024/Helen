using HelenServer.Core;
using HelenServer.Dapr.DataProtection;
using HelenServer.UserCenter.Dal;

var builder = WebApplication.CreateBuilder(args);

builder.AddCloudService();
builder.AddCloudDaprService();

builder.Services.AddDataProtection().PersistKeysToDaprState();
builder.Services.AddCloudIdentityServer(builder.Configuration);
builder.Services.AddDbContext<UserCenterDbContext>();
builder.Services.AddNonBreakingSameSiteCookies();
builder.Services.AddCors(opts => builder.Configuration.Bind(opts));

var app = builder.Build();

app.UseCloudService();
app.UseCloudDaprService();

SqlServerExtensions.Config(builder =>
{
    builder.Options.DbContext = app.Services.GetRequiredService<UserCenterDbContext>();
    builder.Options.GrpcClientFactory = app.Services.GetRequiredService<IGrpcClientFactory>();
});

app.ApplyDataInitialize();

app.UseCors();
app.UseCookiePolicy();

app.UseCloudIdentityServer();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();