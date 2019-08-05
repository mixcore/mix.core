'use strict';
app.factory('PagePositionService', ['BaseODataService',
    function (baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('page-position/portal');
        // Define more service methods here
    return serviceFactory;
}]);
