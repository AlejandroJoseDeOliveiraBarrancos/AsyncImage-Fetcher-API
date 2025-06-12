using AsyncImage_Fetcher_Service.Adapters.Api;
using AsyncImage_Fetcher_Service.Adapters.Api.Middleware;
using FluentValidation.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation();


builder.Services.AddControllers();
builder.Services.AddFluentValidation(config =>
    config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
