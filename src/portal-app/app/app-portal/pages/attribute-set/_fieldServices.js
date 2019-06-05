'use strict';
app.factory('AttributeFieldService', ['BaseService',
    function (baseService) {
        baseService.init('attribute-field');
        var serviceFactory = baseService;
        // Define more service methods here
    return serviceFactory;
}]);
