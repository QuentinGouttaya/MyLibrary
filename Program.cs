using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var connectionString = "Data Source=media.db";
builder.Services.AddDbContext<MediaDb>(options =>
    options.UseSqlite(connectionString));

// Add controllers
builder.Services.AddControllers();

// Swagger/OpenAPI setup service
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Media API", Version = "v1" });
});

var app = builder.Build();

// Run DB migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MediaDb>();
    db.Database.Migrate();
}

// Enable Swagger in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Media API v1"));
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();