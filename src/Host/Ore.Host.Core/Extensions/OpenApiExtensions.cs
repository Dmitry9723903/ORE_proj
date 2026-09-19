using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using Ore.Platform.Time;

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

            services.AddOpenApi(options =>

                // Типы-значения Platform передаются строкой, своим
                // конвертером, и билдеру описания он не виден: у
                // ObservedAt в схеме оставался только текст документации,
                // у BusinessDate — пустой объект. Потребитель из такого
                // описания не узнаёт о форме значения НИЧЕГО, то есть
                // описание не корректно в отсутствии описания.
                options.AddSchemaTransformer((schema, context, _) =>
                {
                    if (WireFormat(context.JsonTypeInfo.Type) is { } format)
                    {
                        schema.Type = JsonSchemaType.String;
                        schema.Format = format;
                    }

                    return Task.CompletedTask;
                }));

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

        /// <summary>
        /// Каким форматом строки тип-значение передается. Перечень
        /// закрыт и пополняется осознанно.
        /// </summary>
        /// <param name="type">Тип, попавший в описание.</param>
        /// <returns>Формат строки или <c>null</c>, если тип обычный.</returns>
        private static string? WireFormat(Type type) => (Nullable.GetUnderlyingType(type) ?? type) switch
        {
            var t when t == typeof(ObservedAt) => "date-time",
            var t when t == typeof(OccurredAt) => "date-time",
            var t when t == typeof(BusinessDate) => "date",
            _ => null,
        };
    }
}
