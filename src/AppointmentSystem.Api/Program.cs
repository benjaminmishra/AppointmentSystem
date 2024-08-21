using System.Text.Json;
using AppointmentSystem.Api;
using AppointmentSystem.Application;
using AppointmentSystem.Infrastructure;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder();
builder.Services.Configure<JsonOptions>(o =>
{
    o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    o.SerializerOptions.Converters.Add(new DateTimeConverter());
});
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o=>{
        o.DocumentSettings = s => {
            s.Title = "Appointment System Calendar Query API";
            s.Version = "v1";
        };
    });

builder.Services.AddApplication();
builder.Services.AddInfrastructure(options => 
    builder.Configuration.GetSection(DatabaseOptions.Section).Bind(options));

var app = builder.Build();
app
    .UseFastEndpoints()
    .UseSwaggerGen();

await app.RunAsync();