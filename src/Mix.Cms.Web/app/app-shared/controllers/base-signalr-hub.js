function BaseHub($scope) {
    $scope.isLog = true;
    $scope.connection = null;
    $scope.host = "/";
    $scope.responses = [];
    $scope.requests = [];
    $scope.rooms = [];
    $scope.others = [];

    $scope.connect = function () {
        $scope.connection.invoke('join', $scope.player);
    };
    $scope.sendMessage = function () {
        $scope.connection.invoke('SendMessage', $scope.request);
    };
    $scope.receiveMessage = function (msg) {
        $scope.responses.splice(0, 0,msg);
        $scope.$applyAsync();
    };
    $scope.prettyJsonObj = function(obj) {
        return JSON.stringify(obj, null, '\t');
    }
    // Starts a connection with transport fallback - if the connection cannot be started using
    // the webSockets transport the function will fallback to the serverSentEvents transport and
    // if this does not work it will try longPolling. If the connection cannot be started using
    // any of the available transports the function will return a rejected Promise.
    $scope.startConnection = function (hubName) {

        $scope.connection = new signalR.HubConnectionBuilder()
            .withUrl($scope.host + hubName)
            .configureLogging(signalR.LogLevel.Information)
            .build();
        // Create a function that the hub can call to broadcast messages.

        $scope.connection.on("ReceiveMessage", (resp) => {
            $scope.receiveMessage(resp);
        });
        $scope.connection.start()
            .then(function () {
                console.log('connection started', $scope.connection);

                //$scope.$apply();
            })
            .catch(function (error) {
                console.log(`Cannot start the connection use transport.`, error);
                return Promise.reject(error);
            });
    };

    $scope.$on("$destroy", function () {
        $scope.connection.stop();
    });
};