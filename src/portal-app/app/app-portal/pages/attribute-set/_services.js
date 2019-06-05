'use strict';
app.factory('AttributeSetService', ['BaseService',
    function (baseService) {
        baseService.init('attribute-set');
        var serviceFactory = baseService;
        // Define more service methods here
    return serviceFactory;
}]);
