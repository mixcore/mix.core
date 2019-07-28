'use strict';
app.factory('ArticleAttributeDataService', ['BaseService',
    function (baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('article-attribute-data');
        // Define more service methods here
    return serviceFactory;
}]);
