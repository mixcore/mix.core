'use strict';
app.factory('ModuleService', ['BaseService',
    function (baseService) {
        baseService.init('module')
        var serviceFactory = baseService;
        // Define more service methods here
        serviceFactory.modelName = 'module';
        serviceFactory.prefixUrl = '/' + serviceFactory.lang + '/module';

        return serviceFactory;
    }]);
