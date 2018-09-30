'use strict';
app.factory('ModuleService', ['BaseService', 
    function (baseService) {
    var serviceFactory = Object.create(baseService);
    // Define more service methods here
    serviceFactory.modelName = 'module';
    serviceFactory.prefixUrl = '/' + serviceFactory.lang + '/module';

    return serviceFactory;
}]);
