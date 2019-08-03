'use strict';
app.factory('PagePositionService', ['BaseService',
    function (baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('page-position');
        // Define more service methods here
    return serviceFactory;
}]);
