'use strict';
app.factory('FileService', ['BaseService',
    function (baseService) {

        var serviceFactory = Object.create(baseService);
        serviceFactory.init('file', true);        
        return serviceFactory;

    }]);
