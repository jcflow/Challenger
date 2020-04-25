using System;
using Newtonsoft.Json;

namespace Hub.Models
{
    /// <summary>
    ///     <see href="https://notifications.spec.whatwg.org/#dictdef-notificationaction">Notification API Standard</see>
    /// </summary>
    public class NotificationAction
    {

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
