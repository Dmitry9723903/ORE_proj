using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ore.Host.Core.Extensions
{
    /// <summary>
    /// Описание опубликованного контракта и страница для его просмотра.
    /// </summary>
    public static class OpenApiExtensions
    {
        /// <summary>
        /// Адрес документа OpenAPI, который отдаёт <c>MapOpenApi</c>.
        /// </summary>
        private const string DocumentPath = "/openapi/v1.json";

        /// <summary>
        /// документ OpenAPI по типам контроллеров.
        /// </summary>
        /// <param name="services">Набор служб хоста.</param>
        /// <returns>возвращает набор служб.</returns>
        public static IServiceCollection AddOreOpenApi(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddOpenApi();

            return services;
        }

        /// <summary>
        /// Публикует документ и страницу его просмотра. Только вне
        /// среды Production: описание контракта наружу не выставляется.
        /// </summary>
        /// <param name="app">Приложение хоста.</param>
        /// <param name="environment">Среда выполнения.</param>
        /// <returns>возвращает приложение.</returns>
        public static WebApplication UseOreOpenApi(this WebApplication app, IHostEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(app);
            ArgumentNullException.ThrowIfNull(environment);

            if (!environment.IsProduction())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(DocumentPath, "ORE v1");
                    options.DocumentTitle = "ORE — опубликованный контракт";
                });
            }

            return app;
        }
    }
}
