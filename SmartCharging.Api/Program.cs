using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using SmartCharging.Api;
using SmartCharging.Api.Data;
using SmartCharging.Api.Endpoints;
using SmartCharging.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddValidators();
builder.Services.AddAppServices();
builder.Services.AddRepositories();

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