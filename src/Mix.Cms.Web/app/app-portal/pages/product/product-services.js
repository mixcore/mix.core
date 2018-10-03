'use strict';
app.factory('ProductService', ['BaseService',
    function (baseService) {
        var serviceFactory = Object.create(baseService);
        serviceFactory.init('product');
        var _getSingle = function () {

        }
        // Define more service methods here
        serviceFactory.getSingle = _getSingle;
        return serviceFactory;
    }]);
