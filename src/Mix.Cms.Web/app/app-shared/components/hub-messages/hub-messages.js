
modules.component('hubMessages', {
    templateUrl: '/app/app-shared/components/hub-messages/hub-messages.html',
    controller: 'HubMessagesController',
    bindings: {
        
    }
});
app.controller('HubMessagesController', ['$scope', function($scope){
    BaseHub.call(this, $scope);
    $scope.init = function(){
        $scope.startConnection('portalhub');
    }
}]);
