'use strict';
app.factory('ArticleAttributeValueService', ['BaseService',
    function (baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('article-attribute-value');
        // Define more service methods here
    return serviceFactory;
}]);
