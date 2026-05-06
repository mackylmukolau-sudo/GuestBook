using Microsoft.EntityFrameworkCore;
using GuestBook.Data;
using GuestBook.Services;

namespace GuestBook.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Реєструє контекст БД та всі сервіси застосунку GuestBook.
    /// Викликати у Program.cs: builder.Services.AddGuestBookServices(builder.Configuration);
    /// </summary>
    public static IServiceCollection AddGuestBookServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Контекст бази даних
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure()
            ));

        // Сервісний шар
        services.AddScoped<IUserService,    UserService>();
        services.AddScoped<IMessageService, MessageService>();

        return services;
    }
}
