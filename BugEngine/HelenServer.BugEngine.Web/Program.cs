using HelenServer.BugEngine.Dal;
using HelenServer.Data.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddCloudService();
builder.AddCloudDaprService();
builder.Services.AddAutoMapper<BugEngineProfile>();
builder.Services.AddDbContextPool<BugEngineDbContext>(optsBuilder =>
{
    optsBuilder.EnableDetailedErrors().EnableSensitiveDataLogging().UseLazyLoadingProxies().UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll).UseSqlServer(builder.Configuration.GetConnectionString("master")).AddInterceptors(new DefaultIntercepter());
});

builder.Services.AddCors(opts => builder.Configuration.Bind(opts));

builder.Services.AddSingleton<IReadWriteSplittingDbContextFactory<BugEngineDbContext>, ReadWriteSplittingDbContextFactory<BugEngineDbContext>>();

var app = builder.Build();

Helen.Services = app.Services;

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCloudService();
app.UseCloudDaprService();

app.UseCors();
app.UseCookiePolicy();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();