namespace Heus.AspNetCore;
public static class AspNetCoreServiceCollectionExtensions
{
    public static IServiceCollection AddAspNetCoreServices(
        this IServiceCollection services)
    {
        // Add services to the container.

        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
}