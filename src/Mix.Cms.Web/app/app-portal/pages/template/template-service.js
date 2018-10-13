'use strict';
app.factory('TemplateService', ['$rootScope', 'CommonService', 'BaseService',
    function ($rootScope, commonService, baseService) {

        var serviceFactory = Object.create(baseService);
        serviceFactory.init('template');        
        return serviceFactory;

    }]);
