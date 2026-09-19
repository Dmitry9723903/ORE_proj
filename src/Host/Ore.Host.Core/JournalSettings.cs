using System;
using Microsoft.Extensions.Configuration;
using Serilog.Events;

namespace Ore.Host.Core
{
    /// <summary>
    /// Настройки логирования, прочитанные из раздела <c>Journal</c>.
    /// Отсутствие раздела отказом не является.
    /// </summary>
    /// <param name="Level">Наш собственный порог записи.</param>
    /// <param name="File">Путь к файлу; дата подставляется ротацией.</param>
    /// <param name="RetainedDays">Сколько суточных файлов хранить.</param>
    public sealed record JournalSettings(
        LogEventLevel Level,
        string File,
        int RetainedDays)
    {
        /// <summary>
        /// Значения, применяемые когда раздела <c>Journal</c> нет.
        /// </summary>
        public static JournalSettings Default { get; } =
            new(LogEventLevel.Information, "logs/ore-.log", 14);

        /// <summary>
        /// Читает настройки логирования.
        /// </summary>
        /// <param name="configuration">Конфигурация хоста.</param>
        /// <returns>Настройки логирования.</returns>
        public static JournalSettings From(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            var section = configuration.GetSection("Journal");
            if (!section.Exists())
            {
                return Default;
            }

            return new JournalSettings(
                Enum.TryParse<LogEventLevel>(section["Level"], ignoreCase: true, out var level)
                    ? level
                    : Default.Level,
                section["File"] is { Length: > 0 } file ? file : Default.File,
                int.TryParse(section["RetainedDays"], out var days) ? days : Default.RetainedDays);
        }
    }
}
