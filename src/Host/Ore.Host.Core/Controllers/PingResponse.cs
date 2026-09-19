namespace Ore.Host.Core.Controllers
{
    /// <summary>
    /// Ответ на запрос проверки связи.
    /// </summary>
    /// <param name="Status">Признак готовности процесса.</param>
    /// <param name="Version">Версия сборки хоста.</param>
    public sealed record PingResponse(string Status, string Version);
}
