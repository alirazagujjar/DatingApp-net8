using API.Data;
using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationServoce(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

using var scope = app.Services.CreateScope();
var service = scope.ServiceProvider;
try{
    var context = service.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);

}
catch(Exception ex){
    var logger = service.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex,"An error occure during Migrations");
}
app.MapControllers();

app.Run();
