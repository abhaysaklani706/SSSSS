using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseCors();

// Enable Swagger in all environments for API testing
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapHub<AdminHub>("/adminHub");
// Also map the agent hub path expected by some agents
app.MapHub<AdminHub>("/agentHub");

// Bind to all interfaces so remote agents can connect
// Listen on 8060 to align with your ngrok forwarding target
// Get port from Railway environment variable or default to 5030 for local development
var port = Environment.GetEnvironmentVariable("PORT") ?? "5030";
app.Run($"http://0.0.0.0:{port}");


// Minimal hub for Admin real-time updates
public class AdminHub : Hub {}
