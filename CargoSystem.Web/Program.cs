using CargoSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
// Aşağıdaki using'leri eklemelisiniz:
using CargoSystem.Application.Services;      // ICargoService için
using CargoSystem.Infrastructure.Services;   // CargoService implementasyonu için

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Veritabanı Bağlantısı
// Microsoft.EntityFrameworkCore.InMemory paketi yüklü olmalıdır.
builder.Services.AddDbContext<CargoSystemDbContext>(options =>
	options.UseInMemoryDatabase("CargoSystemDb"));

// --- DÜZELTME: Servis kayıtları builder.Build() işleminden ÖNCE yapılmalıdır ---
builder.Services.AddScoped<ICargoService, CargoService>();

// NOT: IShippingService ve ShippingService dosyalarınız mevcut değilse aşağıdaki satır hata verir.
// Eğer henüz yazmadıysanız yorum satırı yapın:
// builder.Services.AddScoped<IShippingService, ShippingService>();

var app = builder.Build(); // Uygulama burada inşa edilir

// 2. Seed Data Çalıştırma
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