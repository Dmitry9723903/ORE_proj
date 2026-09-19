using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Ore.Platform.Time;

namespace Ore.Host.Core.Controllers
{
    /// <summary>
    /// Показывает, чем различаются три времени события, полученные из одного
    /// источника данных.
    /// </summary>
    [ApiController]
    [Route("api/times")]
    public sealed class TimesController : ControllerBase
    {
        /// <summary>
        /// Строит три времени из момента, сообщённого источником.
        /// </summary>
        /// <param name="occurred">Момент события со смещением по часовому поясу источника.</param>
        /// <param name="businessDate">День, сообщённый источником.</param>
        /// <returns>Ответ <see cref="TimesResponse"/>.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(TimesResponse), StatusCodes.Status200OK)]
        public ActionResult<TimesResponse> Get(
            [FromQuery][BindRequired] DateTimeOffset occurred,
            [FromQuery] DateOnly? businessDate) =>
            Ok(new TimesResponse(
                ObservedAt.From(occurred),
                OccurredAt.From(occurred),
                businessDate is { } day ? BusinessDate.From(day) : null));
    }
}
