'use strict';
app.factory('PostAttributeDataService', ['BaseService',
    function (baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('post-attribute-data');
        // Define more service methods here
    return serviceFactory;
}]);
