using System;
namespace Hub
{
    public class PushSubscriptionMessage
    {
        public string pushEndpoint { get; set; }

        public string p256dh { get; set; }

        public string auth { get; set; }

        public PushSubscriptionMessage()
        {
        }
    }
}
