using Infomatrix.Core.Api.Extensions;
using Infomatrix.Core.Api.Middlewares;

var builder = WebApplication
    .CreateBuilder(args);

builder.Services
    .AddApiServices(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

var app = builder
    .Build();

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
