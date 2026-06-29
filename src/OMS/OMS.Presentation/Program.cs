using Microsoft.EntityFrameworkCore;
using OMS.Application;
using OMS.Common.Interfaces.Entity;
using OMS.Infrastructure;
using OMS.Presentation;
using OMS.Presentation.Auth;
using OMS.Presentation.Extensions;
using OMS.Presentation.Middleware;
using Scalar.AspNetCore;

const string DevelopmentCorsPolicy = nameof(DevelopmentCorsPolicy);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration);

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services.AddValidation();
builder.Services.AddProblemDetails();
builder.Services.AddRateLimiter();
builder.Services.AddRequestTimeouts();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(DevelopmentCorsPolicy, policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
//builder.Services.AddResponseCaching();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.WithPolymorhicModifiersOf<IConnectorEntity>();
});

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

var app = builder.Build();

// Apply pending migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

    if (pendingMigrations.Any())
    {
        await dbContext.Database.MigrateAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseCors(DevelopmentCorsPolicy);
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseHealthChecks("/health");
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.UseRequestTimeouts();
app.MapControllers();
//app.UseResponseCaching();
app.UseExceptionHandler();
app.MapScalarApiReference();

app.Run();