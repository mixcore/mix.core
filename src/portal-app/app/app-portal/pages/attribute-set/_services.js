'use strict';
app.factory('AttributeSetService', ['BaseService',
    function (baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('attribute-set');
        // Define more service methods here
    return serviceFactory;
}]);
