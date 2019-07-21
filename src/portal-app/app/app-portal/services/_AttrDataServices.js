'use strict';
app.factory('AttributeDataService', ['BaseService',
    function (baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('attribute-data');
        // Define more service methods here
    return serviceFactory;
}]);
