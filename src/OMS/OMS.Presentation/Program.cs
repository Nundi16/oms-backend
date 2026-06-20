using OMS.Application;
using OMS.Application.Connectors;
using OMS.Infrastructure;
using OMS.Presentation;
using OMS.Presentation.Serialization;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration);

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddValidation();
builder.Services.AddProblemDetails();

builder.Services.AddRateLimiter();
builder.Services.AddRequestTimeouts();
//builder.Services.AddExceptionHandler<>();
//builder.Services.AddResponseCaching();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("DevelopmentAll", policy => policy
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
    });
}

builder.Services.AddControllers().AddJsonOptions(option =>
    PolymorphicJsonOption.Configure(option.JsonSerializerOptions, typeof(BaseConnectorDto)));

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("DevelopmentAll");
}

app.UseHttpsRedirection();
app.UseHealthChecks("/health");
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.UseRequestTimeouts();
app.MapControllers();
//app.UseResponseCaching();
//app.UseExceptionHandler();
app.MapScalarApiReference();

app.Run();