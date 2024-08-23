using API.Data;
using API.Helpers;
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
        // configure API controllers
        services.AddControllers();
        // Configure Database
        services.AddDbContext<DataContext>(opt =>
        {
            _ = opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        // Add cors
        services.AddCors(options =>
                {
                    options.AddPolicy("AllowSpecificOrigin",
                        builder => builder.WithOrigins("http://localhost:4200")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod());
                });
                // DI parent and chlid class
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IPhotoService,PhotoService>();
        // Action filter register in DI
        services.AddScoped<LogUserActivity>();
        // Auto,apper configure
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        // Cloudinary credentials mapp on custom class
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
        
        return services;
    }
}