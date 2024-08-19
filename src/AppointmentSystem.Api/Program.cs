using AppointmentSystem.Application;
using AppointmentSystem.Infrastructure;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder();
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(x => builder.Configuration.GetSection(DatabaseOptions.Section));

var app = builder.Build();
app
    .UseFastEndpoints()
    .UseSwaggerGen();

await app.RunAsync();