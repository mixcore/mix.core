'use strict';
app.factory('ProductService', ['BaseService',
    function (baseService) {
        var serviceFactory = Object.create(baseService);
        serviceFactory.init('product');
        return serviceFactory;
    }]);
