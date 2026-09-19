using System;
using Microsoft.Extensions.Logging;

namespace Ore.Host.Core.Extensions
{
    /// <summary>
    /// Настройка логирования.
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// Заводит логи: вывод в консоль, (в планах в файл и в опенсерч), время в UTC.
        /// </summary>
        /// <param name="builder">билдер логов хоста.</param>
        /// <returns>реализатор-билдер.</returns>
        public static ILoggingBuilder AddOreLogging(this ILoggingBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ClearProviders();
            builder.AddSimpleConsole(options =>
            {
                options.SingleLine = true;
                options.UseUtcTimestamp = true;
                options.TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fffZ ";
            });

            return builder;
        }
    }
}
