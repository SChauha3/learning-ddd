using LearningDDD.Api;
using LearningDDD.Api.Endpoints;
using LearningDDD.Api.Extensions;
using LearningDDD.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddValidators();
builder.Services.AddAppServices();
builder.Services.AddRepositories();
builder.Services.AddSqlLite();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // This maps the endpoint for the OpenAPI JSON

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "My .NET 9 Minimal API v1");
        options.RoutePrefix = "swagger"; // This means you'll access the UI at /swagger
    });
}

//app.UseHttpsRedirection();

app.MapEndpoints();

app.MapGet("/health/live", () => Results.Ok("Alive"));
app.MapGet("/health/ready", () => Results.Ok("Ready"));

app.Run();