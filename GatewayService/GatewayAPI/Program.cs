using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCustomAuthentication();
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
await app.UseOcelot();

app.UseAuthentication();
app.UseAuthorization();

app.Run();


