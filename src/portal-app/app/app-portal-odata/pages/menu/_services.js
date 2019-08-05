'use strict';
app.factory('PositionService', ['BaseODataService',
    function (baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('position', 'portal');
        // Define more service methods here
    return serviceFactory;
}]);
