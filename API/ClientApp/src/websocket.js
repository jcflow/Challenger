const scheme = document.location.protocol === "https:" ? "wss" : "ws";
const port = document.location.port ? (":" + document.location.port) : "";

const socket = new WebSocket(scheme + "://" + document.location.hostname + port + "/ws");

socket.onopen = function (event) {
    console.log(event.reason);
};

socket.onclose = function (event) {
    console.log(event.reason);
};

socket.onerror = function (event) {
    console.error(event.reason);
};

export default socket;