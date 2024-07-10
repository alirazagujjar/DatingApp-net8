using API.Data;
using API.interfaces;
using API.Service;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationServoce(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.AddControllers();
        services.AddDbContext<DataContext>(opt =>
        {
            _ = opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCors(options =>
                {
                    options.AddPolicy("AllowSpecificOrigin",
                        builder => builder.WithOrigins("http://localhost:4200")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod());
                });
        services.AddScoped<ITokenService, TokenService>();
        return services;
    }
}