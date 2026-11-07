using CommerceAI.API.ExceptionHandling;
using CommerceAI.API.Middleware;
using CommerceAI.Application.DependencyInjection;
using CommerceAI.Application.Features.Products.CreateProduct;
using CommerceAI.Application.Metrics;
using CommerceAI.Infrastructure.DependencyInjection;
using CommerceAI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(CreateProductCommand).Assembly);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("commerceAI-cors-policy", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService("CommerceAI.API",serviceVersion:"1.01.0"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()

        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri("http://localhost:18889");
        }))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRuntimeInstrumentation()
        .AddMeter(CommerceAIMetrics.MeterName)
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri("http://localhost:18889");
        }));

builder.Services.AddSingleton<ICommerceAIMetrics, CommerceAIMetrics>();

var app = builder.Build();

app.UseCors("commerceAI-cors-policy");

app.UseMiddleware<TraceEnrichmentMiddleware>();
app.MapHealthChecks("health/ready");

app.UseSerilogRequestLogging();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.MapControllers();

app.UseHttpsRedirection();
app.Run();
public partial class Program
{
}
