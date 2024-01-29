const connection = new signalR.HubConnectionBuilder()
    .withUrl("/devicesHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("ReceiveMessage", (message) => {
    $('#devices-panel')
        .prepend($('<div />').text(message));
});

$('#btn-restart').click(function () {
    connection.stream("RestartSystem")
        .subscribe({
            next: (message) => $('#devices-panel')
                .prepend($('<div />')
                    .text(message))
        });
});

async function start() {
    try {
        await connection.start();
        var subject = new signalR.Subject();
        connection.send("BroadcastInitializationSequence", subject)
            .catch(err => console.error(err.toString()));
        subject.next("Device Manager is connecting");
        subject.next("Device Manager is connected");
        subject.complete();
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};

connection.onclose(async () => {
    await start();
});

start();