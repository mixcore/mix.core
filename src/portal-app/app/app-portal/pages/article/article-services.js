'use strict';
app.factory('ArticleService', ['BaseService',
    function (baseService) {
        baseService.init('article');
        var serviceFactory = baseService;
        // Define more service methods here
    return serviceFactory;
}]);
