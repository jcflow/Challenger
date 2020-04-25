const check = () => {
    const serviceWorker = ('serviceWorker' in navigator);
    const pushManager = ('PushManager' in window);
    if (!serviceWorker) {
        console.log('No Service Worker support!')
    }
    if (!pushManager) {
        console.log('No Push API Support!')
    }
    return serviceWorker && pushManager;
}

const registerServiceWorker = () => {
    console.log("registerServiceWorker");
    return navigator.serviceWorker.register('sw.js');
}

const requestNotificationPermission = async () => {
    console.log("requestPermission");
    const permission = await window.Notification.requestPermission();
    // value of permission can be 'granted', 'default', 'denied'
    // granted: user has accepted the request
    // default: user has dismissed the notification permission popup by clicking on x
    // denied: user has denied the request.
    if (permission !== 'granted') {
        console.log('Permission not granted for Notification');
    }
    console.log("permission", permission);
}

const main = async () => {
    if (check()) {
        registerServiceWorker()
            .then(() => requestNotificationPermission());
    }
}

export default main as Function;
