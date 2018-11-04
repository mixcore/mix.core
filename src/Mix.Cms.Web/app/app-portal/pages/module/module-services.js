'use strict';
app.factory('ModuleService', ['BaseService',
    function (baseService) {        
        var serviceFactory = Object.create(baseService);;
        serviceFactory.init('module')
        // Define more service methods here
        return serviceFactory;
    }]);
