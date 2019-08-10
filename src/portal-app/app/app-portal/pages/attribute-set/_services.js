'use strict';
app.factory('AttributeSetService', ['BaseODataService',
    function (baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('attribute-set', true);
        // Define more service methods here
    return serviceFactory;
}]);
