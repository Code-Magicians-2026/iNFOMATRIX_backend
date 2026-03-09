using Infomatrix.Core.Api.Extensions;
using Infomatrix.Core.Api.Middlewares;
using Infomatrix.Core.Application.Extensions;
using Infomatrix.Core.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication
    .CreateBuilder(args);

builder.Services
    .AddApiServices(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

var app = builder
    .Build();

// Apply migrations
//await using (var scope = app.Services.CreateAsyncScope())
//{
//    try
//    {
//        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//        await db.Database.MigrateAsync();
//    }
//    catch (Exception ex)
//    {
//        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
//        logger.LogError(ex, "An error occurred while migrating the database");
//        throw;
//    }
//}

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Infomatrix API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.MapControllers();

await app.RunAsync();
