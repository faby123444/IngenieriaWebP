using IngenieriaWebP.Models; // Asegúrate de usar el espacio de nombres correcto para tus modelos
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar DbContext con la cadena de conexión apropiada
builder.Services.AddDbContext<IngenieriaWebContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IngenieriaWebConnection")));

// Agregar soporte para sesiones
builder.Services.AddDistributedMemoryCache(); // Utiliza el caché en memoria. Para producción, considera otras implementaciones.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Configura el tiempo de expiración de la sesión.
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Marca la cookie de sesión como esencial.
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Usa una página de error genérica en producción.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Importante: asegúrate de llamar a UseSession() antes de UseRouting() y UseEndpoints().

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
