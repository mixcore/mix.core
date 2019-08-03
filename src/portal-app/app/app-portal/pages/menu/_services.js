'use strict';
app.factory('PositionService', ['BaseService',
    function (baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('position');
        // Define more service methods here
    return serviceFactory;
}]);
