using System;
using Hub.Models;

namespace API.Controllers.Models
{
    /// <summary>
    /// Request body model for Push registration
    /// </summary>
    public class PushSubscriptionModel
    {
        /// <inheritdoc cref="Subscription"/>
        public Subscription Subscription { get; set; }

        /// <summary>
        /// Other attributes, like device id for example.
        /// </summary>
        public string DeviceId { get; set; }
    }
}
