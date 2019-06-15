'use strict';
app.factory('ArticleService', ['BaseService',
    function (baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('article');
        // Define more service methods here
    return serviceFactory;
}]);
