using API.Controllers.Models;
using Hub;
using Microsoft.AspNetCore.Mvc;
using WebPush;

namespace API.Controllers
{

    /// <summary>
    /// VAPID Push Notification API
    /// </summary>
    [Produces("application/json")]
    [Route("push")]
    [ApiController]
    public class PushController : ControllerBase
    {
        // GET: api/push/vapidpublickey
        /// <summary>
        /// Get VAPID Public Key
        /// </summary>
        /// <returns>VAPID Public Key</returns>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet, Route("vapidpublickey")]
        public ActionResult<string> GetVapidPublicKey()
        {
            return Ok(PushHub.GetVapidPublicKey());
        }

        // POST: api/push/subscribe 
        /// <summary>
        /// Subscribe for push notifications
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">NoContent</response>
        /// <response code="400">BadRequest if subscription is null or invalid.</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("subscribe")]
        public ActionResult<PushSubscription> Subscribe([FromBody] PushSubscriptionModel model)
        {
            PushHub.Subscribe(model.Subscription);
            return Ok();
        }

        // POST: api/push/unsubscribe
        /// <summary>
        /// Unsubscribe for push notifications
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="204">NoContent</response>
        /// <response code="400">BadRequest if subscription is null or invalid.</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("unsubscribe")]
        public ActionResult Unsubscribe([FromBody] PushSubscriptionModel model)
        {
            PushHub.Unsubscribe(model.Subscription);
            return Ok();
        }
    }
}
