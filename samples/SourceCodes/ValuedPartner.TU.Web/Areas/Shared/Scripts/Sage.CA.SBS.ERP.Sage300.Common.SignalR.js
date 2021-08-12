/* Copyright (c) 2019-2021 Sage Software, Inc.  All rights reserved. */

var SignalR = {
    //Initialize and establish connection to SignalR
    initialize: function (id) {
        var connection = $.hubConnection(`/${window.location.pathname.split('/')[1]}/sagesignalr`, { useDefaultPath: false });
        connection.qs = { 'key': id };
        var signalRHub = connection.createHubProxy('SageSignalRHub')

        //Hub methods
        signalRHub.on('evict', SignalR.onServerCall.evict);

        // Start the connection.
        SignalR.startConnection(connection);
    },

    startConnection: function (connection) {
        connection.start()
            .done(function () { console.log("Connection Established"); })
            .fail(function () {
                console.log('Connection failed. Retrying...');
                setTimeout(function () { SignalR.startConnection(connection); }, 3000);
            });
    },

    //Client methods the server can call
    onServerCall: {
        evict: function() {
            sg.utls.userEviction();
        }
    }
};
