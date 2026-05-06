using Microsoft.EntityFrameworkCore;
using GuestBook.Data;
using GuestBook.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Реєстрація БД та сервісного шару через extension-метод
builder.Services.AddGuestBookServices(builder.Configuration);

// Сесія (стан авторизації)
builder.Services.AddSession(options =>
{
    options.IdleTimeout        = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly    = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Автоматичне застосування міграцій при старті
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name:    "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
