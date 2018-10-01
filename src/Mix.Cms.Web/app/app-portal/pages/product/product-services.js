'use strict';
app.factory('ProductService', ['BaseService',
    function (baseService) {
        baseService.init('product');
        var serviceFactory = baseService;
        // Define more service methods here
        serviceFactory.modelName = 'product';
        serviceFactory.prefixUrl = '/' + serviceFactory.lang;
        return serviceFactory;
    }]);
