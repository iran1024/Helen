using HelenServer.Data.SqlServer;
using HelenServer.FileStorage.Dal;
using HelenServer.FileStorage.FastDFS;

var builder = WebApplication.CreateBuilder(args);

builder.AddCloudService();
builder.AddCloudDaprService();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper<FastDFSProfile>();
builder.Services.AddDistributedFileStorage(builder.Configuration);

builder.Services.AddSingleton<IReadWriteSplittingDbContextFactory<AttachmentDbContext>, ReadWriteSplittingDbContextFactory<AttachmentDbContext>>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCloudService();
app.UseCloudDaprService();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();