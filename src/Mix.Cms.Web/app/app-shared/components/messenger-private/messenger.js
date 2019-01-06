modules.component('messengerPrivate', {
    templateUrl: '/app/app-shared/components/messenger-private/index.html',
    controller: 'MessengerController',
    bindings: {
        message: '=',
        connectionId: '='
    }
});