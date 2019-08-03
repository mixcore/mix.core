'use strict';
app.factory('PostService', ['BaseService',
    function (baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('post');
        // Define more service methods here
    return serviceFactory;
}]);
