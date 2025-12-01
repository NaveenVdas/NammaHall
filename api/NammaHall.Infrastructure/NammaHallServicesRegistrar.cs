using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NammaHall.Application.Handlers.Admin.Auth;
using NammaHall.Application.Handlers.Admin.Booking;
using NammaHall.Application.Handlers.Admin.Hall;
using NammaHall.Application.Handlers.Booking;
using NammaHall.Application.Handlers.Hall;
using NammaHall.Application.QueryServices;
using NammaHall.Application.Repository;
using NammaHall.Application.UnitOfWork;
using NammaHall.Infrastructure.Queries;
using NammaHall.Infrastructure.Repository;

namespace NammaHall.Infrastructure;

public static class NammaHallServicesRegistrar
{
    public static IServiceCollection RegisterNammaHallServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.RegisterInfrastructureServices(configuration)
            .RegisterApplicationServices();
    }

    private static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Render provides DATABASE_URL environment variable for PostgreSQL
        // Also check configuration (appsettings.json) as fallback
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL") ?? 
                         configuration["DATABASE_URL"];
        var databaseProvider = configuration["DatabaseProvider"] ?? 
                              (databaseUrl != null ? "PostgreSQL" : "SqlServer");
        
        var connectionString = databaseProvider switch
        {
            "PostgreSQL" or "Postgres" => ParseDatabaseUrl(databaseUrl) ?? 
                                         configuration.GetConnectionString("PostgreSqlConnection") ?? 
                                         configuration.GetConnectionString("DefaultConnection"),
            _ => configuration.GetConnectionString("DefaultConnection")
        };

        return services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .RegisterRepositories()
            .AddDbContext<NammaHallDbContext>(options =>
            {
                if (databaseProvider == "PostgreSQL" || databaseProvider == "Postgres")
                {
                    options.UseNpgsql(connectionString);
                }
                else
                {
                    options.UseSqlServer(connectionString);
                }
            });
    }

    private static IServiceCollection RegisterApplicationServices(this IServiceCollection services) =>
        services
            .AddScoped<IHallQueries, HallQueries>()
            .AddScoped<IBookingQueries, BookingQueries>()
            .AddScoped<IAdminQueries, AdminQueries>()
            .RegisterHandlers()
            .RegisterRepositories();

    private static IServiceCollection RegisterHandlers(this IServiceCollection services)
    {
        // Public handlers
        services.AddScoped<IHallListHandler, HallListHandler>();
        services.AddScoped<IHallGetHandler, HallGetHandler>();
        services.AddScoped<IHallAvailabilityHandler, HallAvailabilityHandler>();
        services.AddScoped<IBookingCreateHandler, BookingCreateHandler>();
        services.AddScoped<IBookingGetHandler, BookingGetHandler>();

        // Admin handlers
        services.AddScoped<IAdminLoginHandler, AdminLoginHandler>();
        services.AddScoped<IAdminHallListHandler, AdminHallListHandler>();
        services.AddScoped<IAdminHallCreateHandler, AdminHallCreateHandler>();
        services.AddScoped<IAdminHallUpdateHandler, AdminHallUpdateHandler>();
        services.AddScoped<IAdminHallDeleteHandler, AdminHallDeleteHandler>();
        services.AddScoped<IAdminBookingListHandler, AdminBookingListHandler>();
        services.AddScoped<IAdminBookingUpdateHandler, AdminBookingUpdateHandler>();

        return services;
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services) =>
        services
            .AddScoped<IHallRepository, HallRepository>()
            .AddScoped<IBookingRepository, BookingRepository>();

    /// <summary>
    /// Parses Render's DATABASE_URL format (postgresql://user:password@host:port/database or postgresql://user:password@host/database)
    /// into Npgsql connection string format (Host=host;Port=port;Database=database;Username=user;Password=password)
    /// </summary>
    private static string? ParseDatabaseUrl(string? databaseUrl)
    {
        if (string.IsNullOrEmpty(databaseUrl))
            return null;

        // Render format: postgresql://user:password@host:port/database or postgresql://user:password@host/database
        if (databaseUrl.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase) ||
            databaseUrl.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                var uri = new Uri(databaseUrl);
                var userInfo = uri.UserInfo.Split(':');
                var username = userInfo.Length > 0 ? Uri.UnescapeDataString(userInfo[0]) : string.Empty;
                var password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty;
                var host = uri.Host;
                // Internal URLs might not have port, default to 5432
                var port = uri.Port > 0 ? uri.Port : 5432;
                var database = uri.AbsolutePath.TrimStart('/');

                // For internal URLs (no domain), don't require SSL
                var sslMode = host.Contains('.') ? "SSL Mode=Require;Trust Server Certificate=true" : "SSL Mode=Prefer";
                
                return $"Host={host};Port={port};Database={database};Username={username};Password={password};{sslMode}";
            }
            catch
            {
                // If parsing fails, return as-is (might already be in correct format)
                return databaseUrl;
            }
        }

        return databaseUrl;
    }
}

