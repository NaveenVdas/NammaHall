using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NammaHall.Api.Configuration;

public static class CorsRegistration
{
    public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
    {
        // Read from environment variable first (for production), then from config
        var allowedOriginsEnv = Environment.GetEnvironmentVariable("AllowedOrigins");
        var allowedOrigins = !string.IsNullOrEmpty(allowedOriginsEnv)
            ? allowedOriginsEnv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            : configuration.GetRequiredSection("ApplicationSettings").Get<ApplicationSettings>()!.AllowedOrigins;

        return services.AddCors(options => options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.WithOrigins(allowedOrigins);
        }));
    }
}

