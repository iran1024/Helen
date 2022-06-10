using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.AspNetCore.HttpOverrides;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddCloudService(
        this WebApplicationBuilder builder, Action<IServiceOptions>? optionsAction = null)
    {
        builder.Services.Configure<ForwardedHeadersOptions>(opts =>
        {
            opts.KnownProxies.Clear();
            opts.KnownNetworks.Clear();
            opts.ForwardedHeaders = ForwardedHeaders.All;
        });

        builder.Services.AddControllers().AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddHealthChecks();
        builder.Services.AddHttpContextAccessor();

        //builder.Services.AddConfigureOptions();
        builder.Services.AddSwagger(builder.Configuration);
        builder.Services.AddCloudServiceCore(optionsAction);

        return builder;
    }

    public static IServiceCollection AddConfigureOptions(this IServiceCollection services)
    {
        services.AddSingleton<IPostConfigureOptions<ConfigurationOptions>, ConfigureConfigurationOptions>();

        return services;
    }

    public static IServiceCollection AddAutoMapper<TProfile>(this IServiceCollection services)
        where TProfile : Profile, new()
    {
        services.AddAutoMapper(cfg => cfg.AddExpressionMapping().AddProfile<TProfile>(), Assembly.GetExecutingAssembly());

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("Swagger");

        if (section != null && section.GetValue<bool>("IsEnabled"))
        {
            services.AddSwaggerGen(options =>
            {
                string? docPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

                string docDir = Path.Combine(docPath ?? string.Empty, "Docs");

                if (Directory.Exists(docDir))
                {
                    string[] docFiles = Directory.GetFiles(docDir, "*.xml");

                    foreach (string? docFile in docFiles)
                    {
                        options.IncludeXmlComments(docFile, includeControllerXmlComments: true);
                    }
                }

                configuration.Bind(options);
            });
        }

        return services;
    }
}