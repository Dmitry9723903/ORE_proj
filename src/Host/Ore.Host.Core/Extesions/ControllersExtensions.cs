using System;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Ore.Host.Core.Extensions
{
    /// <summary>
    /// Регистрация контроллеров и правил сериализации.
    /// </summary>
    public static class ControllersExtensions
    {
        /// <summary>
        /// Заводит контроллеры. Перечисления - строкой,
        /// а не интом: число меняет смысл при вставке нового
        /// элемента в середину перечисления без сщщбщений.
        /// </summary>
        /// <param name="services">Набор служб хоста.</param>
        /// <returns>Тот же набор служб.</returns>
        public static IServiceCollection AddOreControllers(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services
                .AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            return services;
        }
    }
}
