using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Microsoft.Extensions.Configuration;

public static class ConfigurationExtensions
{
    public static CorsOptions Bind(this IConfiguration configuration, CorsOptions options, string sectionName = "Cors")
    {
        var section = configuration.GetSection(sectionName);

        var policies = section.GetSection("Policies");

        foreach (var c in policies.GetChildren())
        {
            string name = c.GetValue<string>("Name");

            if (string.IsNullOrWhiteSpace(name) ||
                string.Equals(name, "default", StringComparison.OrdinalIgnoreCase))
            {
                options.AddDefaultPolicy(m => m.OnBind(c));
            }
            else
            {
                var builder = new CorsPolicyBuilder();

                builder.OnBind(c);

                options.AddPolicy(name, builder.Build());
            }
        }

        return options;
    }

    private static void OnBind(this CorsPolicyBuilder builder, IConfigurationSection c)
    {
        bool allowAnyHeader = c.GetValue<bool>("AllowAnyHeader");

        bool allowAnyMethod = c.GetValue<bool>("AllowAnyMethod");

        bool allowAnyOrigin = c.GetValue<bool>("AllowAnyOrigin");

        string[] exposedHeaders = c.GetSection("ExposedHeaders").Get<string[]>();

        string[] headers = c.GetSection("Headers").Get<string[]>();

        string[] methods = c.GetSection("Methods").Get<string[]>();

        string[] origins = c.GetSection("Origins").Get<string[]>();

        long preflightMaxAge = c.GetValue<long>("PreflightMaxAge");

        bool supportsCredentials = c.GetValue<bool>("SupportsCredentials");

        if (allowAnyHeader)
        {
            builder.AllowAnyHeader();
        }

        if (allowAnyMethod)
        {
            builder.AllowAnyMethod();
        }

        if (allowAnyOrigin)
        {
            builder.AllowAnyOrigin();
        }

        if (exposedHeaders != null)
        {
            builder.WithExposedHeaders(exposedHeaders);
        }

        if (headers != null)
        {
            builder.WithHeaders(headers);
        }

        if (methods != null)
        {
            builder.WithMethods(methods);
        }

        if (origins != null)
        {
            builder.WithOrigins(origins);
        }

        if (preflightMaxAge > 0)
        {
            builder.SetPreflightMaxAge(TimeSpan.FromMilliseconds(preflightMaxAge));
        }

        if (supportsCredentials)
        {
            builder.AllowCredentials();
        }
    }
}
