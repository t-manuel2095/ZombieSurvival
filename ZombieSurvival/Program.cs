using Microsoft.EntityFrameworkCore;
using ZombieSurvival.Data;
using ZombieSurvival.Services;

var builder = WebApplication.CreateBuilder(args);

// SQL Server + EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Game services
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<ITravelService, TravelService>();
builder.Services.AddScoped<ILootService, LootService>();
builder.Services.AddScoped<IScavengerService, ScavengerService>();
builder.Services.AddScoped<IGunShopService, GunShopService>();
builder.Services.AddScoped<ICampServices, CampServices>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorDev", policy =>
    {
        policy.WithOrigins(
                "https://localhost:7104",  // Blazor https
                "http://localhost:5078")   // Blazor http
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.InjectStylesheet("/swagger-ui/custom.css");
    });
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await GameSeed.SeedAsync(db);
}

app.UseCors("BlazorDev");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
