using Microsoft.EntityFrameworkCore;
using URLShortener.Database;
using DotNetEnv;

Env.Load();

string frontendOrigin = "_frontendOrigin";
string frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost";
string frontendPort = Environment.GetEnvironmentVariable("FRONTEND_PORT") ?? "3000";

Console.WriteLine(frontendUrl + ":" + frontendPort);


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<UrlDbContext>(options =>
    options.UseSqlite("Data Source=urls.db"));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: frontendOrigin, policy =>
    {
    policy.WithOrigins($"{frontendUrl}:{frontendPort}")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(frontendOrigin);

app.MapControllers();

app.Run();
