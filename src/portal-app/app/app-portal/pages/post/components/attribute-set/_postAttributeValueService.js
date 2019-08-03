'use strict';
app.factory('PostAttributeValueService', ['BaseService',
    function (baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('post-attribute-value');
        // Define more service methods here
    return serviceFactory;
}]);
