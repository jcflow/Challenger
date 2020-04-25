// urlB64ToUint8Array is a magic function that will encode the base64 public key
// to Array buffer which is needed by the subscription option
const urlB64ToUint8Array = base64String => {
    const padding = '='.repeat((4 - (base64String.length % 4)) % 4);
    const base64 = (base64String + padding).replace(/\-/g, '+').replace(/_/g, '/');
    const rawData = atob(base64);
    const outputArray = new Uint8Array(rawData.length);
    for (let i = 0; i < rawData.length; ++i) {
        outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
}

const saveSubscription = subscription => {
    const SERVER_URL = './push/subscribe'
    return fetch(SERVER_URL, {
        method: 'post',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            subscription: subscription
        })
    });
}

self.addEventListener('install', (event) => {});

self.addEventListener('activate', async (event) => {
    // This will be called only once when the service worker is installed for first time.
    try {
        const applicationServerKey = urlB64ToUint8Array(
            'BEjygONyShh6iKIVIhqZlVMabbSSaEX6MsXVyA9_9mqXOdxSbSm_26nMrHXKdIsueFsmNDZfOAC-QQGNyMJPKeM'
        );
        const options = { applicationServerKey, userVisibleOnly: true };
        const subscription = await self.registration.pushManager.subscribe(options);
        const isSubscribed = !(subscription === null);
        if (isSubscribed) {
            await saveSubscription(subscription);
        }
    } catch (err) {
        console.error(err);
    }
})

self.addEventListener('push', (event) => {
    if (event.data && event.data.json) {
        showLocalNotification(event, event.data.json(), self.registration);
    }
})

self.addEventListener('notificationclick', (event) => {
    event.notification.close();
    const id = event.notification.data[0];
    event.waitUntil(clients.openWindow(`/tournament/${id}`))
});

self.addEventListener('pushsubscriptionchange', () => {
    const SERVER_URL = './push/unsubscribe'
    fetch(SERVER_URL, {
        method: 'post',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            subscription: subscription
        })
    });
});

const showLocalNotification = (event, data, swRegistration) => {
    const { title, lang, body, tag, timestamp, requireInteraction, actions, image, params } = data;

    const options = {
        body,
        lang,
        requireInteraction,
        tag: tag || undefined,
        timestamp: timestamp ? Date.parse(timestamp) : undefined,
        actions: actions || undefined,
        image: image || undefined,
        badge: '/favicon.ico',
        icon: '/logo.png',
        data: params,
    };
    event.waitUntil(swRegistration.showNotification(title, options));
}
