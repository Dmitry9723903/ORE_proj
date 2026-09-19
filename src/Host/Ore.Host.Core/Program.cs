using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ore.Host.Core.Extensions;
using Serilog;

namespace Ore.Host.Core
{
    /// <summary>
    /// Точка входа ядра.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Собирает и запускает веб-хост.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddOreLogging("ore-core");

            builder.Services.AddOreControllers();
            builder.Services.AddOreCors();
            builder.Services.AddOreOpenApi();
            builder.Services.AddOreHealthChecks();
            builder.Services.AddProblemDetails();

            var app = builder.Build();

            app.UseSerilogRequestLogging();
            app.UseExceptionHandler();
            app.UseStatusCodePages();
            app.UseOreOpenApi(app.Environment);
            app.UseCors(CorsExtensions.AllowAll);
            app.MapControllers();
            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}
