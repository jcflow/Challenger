using System;

namespace Hub.Models
{
    /// <summary>
    /// Representation of the Web Standard Push API's <see href="https://developer.mozilla.org/en-US/docs/Web/API/PushSubscription">PushSubscription</see>
    /// </summary>
    public class Subscription
    {/// <summary>
     /// The endpoint associated with the push subscription.
     /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// The subscription expiration time associated with the push subscription, if there is one, or null otherwise.
        /// </summary>
        public double? ExpirationTime { get; set; }

        /// <inheritdoc cref="Keys"/>
        public Keys Keys { get; set; }

        /// <summary>
        /// Converts the push subscription to the format of the library WebPush
        /// </summary>
        /// <returns>WebPush subscription</returns>
        public WebPush.PushSubscription ToWebPushSubscription() => new WebPush.PushSubscription(Endpoint, Keys.P256Dh, Keys.Auth);
    }

    /// <summary>
    /// Contains the client's public key and authentication secret to be used in encrypting push message data.
    /// </summary>
    public class Keys
    {
        /// <summary>
        /// An <see href="https://en.wikipedia.org/wiki/Elliptic_curve_Diffie%E2%80%93Hellman">Elliptic curve Diffie–Hellman</see> public key on the P-256 curve (that is, the NIST secp256r1 elliptic curve).
        /// The resulting key is an uncompressed point in ANSI X9.62 format.
        /// </summary>
        public string P256Dh { get; set; }

        /// <summary>
        /// An authentication secret, as described in <see href="https://tools.ietf.org/html/draft-ietf-webpush-encryption-08">Message Encryption for Web Push</see>.
        /// </summary>
        public string Auth { get; set; }
    }
}
