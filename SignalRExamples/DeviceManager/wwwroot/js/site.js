const connection = new signalR.HubConnectionBuilder()
    .withUrl("/devicesHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("ReceivePayload", (payload) => {
    $('#devices-panel').prepend($('<div />').append(`Device ${payload.deviceId} connected`));
    $('#devices-panel').prepend($('<div />').append(`Firmware version ${payload.version}`));
});

connection.on("ReceiveDisconnection", (deviceId) => {
    $('#devices-panel').prepend($('<div />').append(`Device ${deviceId} disconnected`));
});

async function start() {
    try {
        await connection.start();
        console.log('connected');
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};

connection.onclose(async () => {
    await start();
});

start();