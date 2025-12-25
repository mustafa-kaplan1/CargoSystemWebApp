using CargoSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Veritabanı Bağlantısı (LocalDB veya InMemory kullanabiliriz geliştirme için)
// Entity Framework Core paketlerinin yüklü olması gerekir.
builder.Services.AddDbContext<CargoSystemDbContext>(options =>
	options.UseInMemoryDatabase("CargoSystemDb")); // Hızlı test için InMemory, kalıcı olması için UseSqlServer

var app = builder.Build();


builder.Services.AddScoped<ICargoService, CargoService>();
builder.Services.AddScoped<IShippingService, ShippingService>();

// 2. Seed Data Çalıştırma (Uygulama her başladığında verileri kontrol et)
using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<CargoSystemDbContext>();
	// InMemory kullanıyorsak her seferinde temiz başlar, bu yüzden seed hep çalışır.
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