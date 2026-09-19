using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ore.Host.Core.Controllers
{
    /// <summary>
    /// Проверка связи с процессом.
    /// </summary>
    [ApiController]
    [Route("api/ping")]
    public sealed class PingController : ControllerBase
    {
        /// <summary>
        /// Возвращает признак готовности и версию.
        /// </summary>
        /// <returns>Ответ <see cref="PingResponse"/>.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PingResponse), StatusCodes.Status200OK)]
        public ActionResult<PingResponse> Get() => Ok(new PingResponse("ok", "0.1.0"));
    }
}
