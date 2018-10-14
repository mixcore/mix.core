'use strict';
app.factory('TemplateService', ['BaseService',
    function (baseService) {

        var serviceFactory = Object.create(baseService);
        serviceFactory.init('template');        
        return serviceFactory;

    }]);
