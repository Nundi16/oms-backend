using OMS.Application;
using OMS.Application.Connectors;
using OMS.Common.Interfaces.Connectors;
using OMS.Common.Interfaces.Extensions;
using OMS.Infrastructure;
using OMS.Presentation;
using OMS.Presentation.Extensions;
using OMS.Presentation.Middleware;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
//builder.Services.AddResponseCaching();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.WithPolymorhicModifiersOf<IConnector>();
    options.JsonSerializerOptions.WithPolymorhicModifiersOf<IExtension>();
});

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseHealthChecks("/health");
app.UseAuthorization();
app.UseRateLimiter();
app.UseRequestTimeouts();
app.MapControllers();
//app.UseResponseCaching();
app.UseExceptionHandler();
app.MapScalarApiReference();

app.Run();