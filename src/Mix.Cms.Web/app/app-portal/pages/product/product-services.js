'use strict';
app.factory('ProductService', ['BaseService', 
    function (baseService) {
    var serviceFactory = Object.create(baseService);
    // Define more service methods here
    serviceFactory.modelName = 'product';
    serviceFactory.prefixUrl = '/' + serviceFactory.lang + '/product';
    return serviceFactory;
}]);
