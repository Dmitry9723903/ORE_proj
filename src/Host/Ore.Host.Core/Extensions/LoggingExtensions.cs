using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Ore.Host.Core.Extensions
{
    /// <summary>
    /// Настройки логирования.
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// Что видно из терминала: коротко, без даты.
        /// </summary>
        private const string ConsoleTemplate =
            "[{Timestamp:HH:mm:ss} {Level:u3}] trace={TraceId} {SourceContext} {Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// Что остаётся в файле: полные даные со смещением по часовому поясу и трэйсбэк.
        /// </summary>
        private const string FileTemplate =
            "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] trace={TraceId} span={SpanId} {SourceContext} {Message:lj}{NewLine}{Exception}";

        /// <summary>
        /// Заводит логирование: консоль и файл с ротацией по суткам.
        /// Идентификатор трассы стоит в обоих назначениях — без него
        /// строки одного обращения не собрать.
        /// </summary>
        /// <param name="builder">Построитель приложения.</param>
        /// <param name="processName">Имя процесса в записях журнала.</param>
        /// <returns>Тот же построитель.</returns>
        public static WebApplicationBuilder AddOreLogging(
            this WebApplicationBuilder builder,
            string processName)
        {
            ArgumentNullException.ThrowIfNull(builder);

            var journal = JournalSettings.From(builder.Configuration);

            var configuration = new LoggerConfiguration()
                .MinimumLevel.Is(journal.Level)

                // Чужой шум приглушается здесь, а не настройкой: это не
                // порог, выбираемый наблюдением, а свойство чужих
                // библиотек. Каждая строка — про конкретный источник.
                // Уровень Error сквозь это проходит, поэтому
                // необработанные исключения в журнале остаются.
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("process", processName)
                .WriteTo.Console(
                    outputTemplate: ConsoleTemplate,
                    formatProvider: CultureInfo.InvariantCulture)
                .WriteTo.File(
                    path: journal.File,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: journal.RetainedDays,
                    outputTemplate: FileTemplate,
                    formatProvider: CultureInfo.InvariantCulture);

            // Log.Logger задаётся, а не только регистрируется в DI:
            // UseSerilogRequestLogging пишет в статический логгер, и без
            // этой строки журнал запросов молча пуст.
            Log.Logger = configuration.CreateLogger();

            builder.Logging.ClearProviders();
            builder.Services.AddSerilog(Log.Logger);

            return builder;
        }
    }
}
