using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ore.Host.Core.Extensions
{
    /// <summary>
    /// Настройка межисточниковых запросов.
    /// </summary>
    public static class CorsExtensions
    {
        /// <summary>
        /// разрешение запросов с любого источника.
        /// </summary>
        public const string AllowAll = "AllowAll";

        /// <summary>
        /// Заводит политику <see cref="AllowAll"/>. Она нужна фронтенду
        /// в будщем, который живёт на своём порту.
        /// </summary>
        /// <param name="services">Набор служб хоста.</param>
        /// <returns>Возвращает набор служб.</returns>
        public static IServiceCollection AddOreCors(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddCors(options => options.AddPolicy(
                AllowAll,
                policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            return services;
        }
    }
}
