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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

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

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
