using CargoSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CargoSystem.Application.Services;
using CargoSystem.Infrastructure.Services;
using CargoSystem.Application.UseCases;
using CargoSystem.Domain.Services;
using CargoSystem.Infrastructure.Maps;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Veritabanı (InMemory)
builder.Services.AddDbContext<CargoSystemDbContext>(options =>
	options.UseInMemoryDatabase("CargoSystemDb"));

// 2. Temel Servisler (Dependency Injection Tanımları)
builder.Services.AddScoped<ICargoService, CargoService>();

// 3. Domain Servisleri ve Algoritmalar
// Dijkstra algoritması bir Graph nesnesine ihtiyaç duyar. Bunu Singleton olarak ekliyoruz.
builder.Services.AddSingleton<CargoSystem.Domain.Graph.Graph>(sp =>
	new KocaeliRoadGraphProvider().GetGraph());

builder.Services.AddScoped<IShortestPathService, DijkstraPathService>();
builder.Services.AddScoped<IRoutePlanner, GreedyRoutePlanner>(); // Greedy veya diğer algoritmanız
builder.Services.AddScoped<IRouteCostCalculator, RouteCostCalculator>();

// 4. Use Case'ler (Admin Controller'ın çalışması için gerekli)
builder.Services.AddScoped<RunScenarioUseCase>();

var app = builder.Build();

// 5. Seed Data (Başlangıç Verileri)
using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<CargoSystemDbContext>();
	DbSeeder.Seed(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();