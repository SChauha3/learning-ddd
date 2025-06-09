using LearningDDD.Api;
using LearningDDD.Api.Endpoints;
using LearningDDD.Api.Extensions;
using LearningDDD.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddValidators();
builder.Services.AddAppServices();
builder.Services.AddRepositories();
builder.Services.AddSqlLite();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapEndpoints();

app.MapGet("/health/live", () => Results.Ok("Alive"));
app.MapGet("/health/ready", () => Results.Ok("Ready"));

app.Run();