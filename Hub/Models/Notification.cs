using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hub.Models
{
    /// <summary>
    ///     <see href="https://notifications.spec.whatwg.org/#dictdef-notificationoptions">Notification API Standard</see>
    /// </summary>
    public class
        Notification
    {
        public Notification() { }

        public Notification(string text)
        {
            Body = text;
        }

        [JsonProperty("title")]
        public string Title { get; set; } = "Challenger";

        [JsonProperty("lang")]
        public string Lang { get; set; } = "en";

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("badge")]
        public string Badge { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [JsonProperty("requireInteraction")]
        public bool RequireInteraction { get; set; } = false;

        [JsonProperty("actions")]
        public List<NotificationAction> Actions { get; set; } = new List<NotificationAction>();

        [JsonProperty("params")]
        public List<string> Params { get; set; } = new List<string>();
    }
}
