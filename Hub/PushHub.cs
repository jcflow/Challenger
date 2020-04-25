using System;
using System.Collections.Generic;
using System.Timers;
using Hub.Models;
using Newtonsoft.Json;
using WebPush;

namespace Hub
{
    public class PushHub
    {
        private static string VAPID_SUBJECT = "mailto:iamjcflow@gmail.com";
        private static string VAPID_PUBLIC_KEY = @"BEjygONyShh6iKIVIhqZlVMabbSSaEX6MsXVyA9_9mqXOdxSbSm_26nMrHXKdIsueFsmNDZfOAC-QQGNyMJPKeM";
        private static string VAPID_PRIVATE_KEY = @"mPc-YW_dRn0_KedsZge9C7XfLof5Rgvs35pqQA_CDYs";

        private static List<Subscription> _subscriptions = new List<Subscription>();
        private static WebPushClient _client = new WebPushClient();
        private static VapidDetails _vapidDetails = new VapidDetails(VAPID_SUBJECT, VAPID_PUBLIC_KEY, VAPID_PRIVATE_KEY);

        private static System.Threading.Timer timer;

        public static void InitTimer()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(10);

            timer = new System.Threading.Timer((e) =>
            {
                Notification n = new Notification() {
                    Title = "New tournament created!",
                    Body = "NAME OF THE TOURNAMENT",
                    Params = { "1" }
                };
                Send(n);
            }, null, startTimeSpan, periodTimeSpan);
        }

        public static string GetVapidPublicKey()
        {
            return VAPID_PUBLIC_KEY;
        }

        public static void Subscribe(Subscription subscription)
        {
            _subscriptions.Add(subscription);
        }

        public static void Unsubscribe(Subscription subscription)
        {
            _subscriptions.Remove(subscription);
        }

        public static async void Send(Notification notification)
        {
            lock (_subscriptions)
            {
                foreach (var subscription in _subscriptions)
                {
                    try
                    {
                        _client.SendNotification(subscription.ToWebPushSubscription(), JsonConvert.SerializeObject(notification), _vapidDetails);
                    }
                    catch (WebPushException e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }
    }
}
