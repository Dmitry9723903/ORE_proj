using Ore.Platform.Time;

namespace Ore.Host.Core.Controllers
{
    /// <summary>
    /// Три времени события, полученные из одного источника данных.
    /// </summary>
    /// <param name="ObservedAt">Момент, приведённый к UTC.</param>
    /// <param name="OccurredAt">Момент со смещением по часовому поясу источника.</param>
    /// <param name="BusinessDate">
    /// День, сообщённый источником; <c>null</c>, если не сообщён.
    /// </param>
    public sealed record TimesResponse(
        ObservedAt ObservedAt,
        OccurredAt OccurredAt,
        BusinessDate? BusinessDate);
}
