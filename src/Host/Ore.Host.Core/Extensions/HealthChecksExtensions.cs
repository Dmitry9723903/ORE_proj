using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ore.Host.Core.Extensions
{
    /// <summary>
    /// Проверки жизни хоста.
    /// </summary>
    public static class HealthChecksExtensions
    {
        /// <summary>
        /// Заводит набор проверок. На этом этапе проверок нет: сам факт
        /// ответа на /health означает, что процесс поднялся и слушает.
        /// Проверки внешних зависимостей добавляются вместе с ними.
        /// </summary>
        /// <param name="services">Набор служб хоста.</param>
        /// <returns>возвращает это же.</returns>
        public static IServiceCollection AddOreHealthChecks(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddHealthChecks();

            return services;
        }
    }
}
