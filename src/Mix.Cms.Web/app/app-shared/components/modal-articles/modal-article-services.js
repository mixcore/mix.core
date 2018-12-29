'use strict';
app.factory('ModalArticleService', ['$rootScope', 'CommonService', 'BaseService',
    function ($rootScope, commonService, baseService) {

        var serviceFactory = Object.create(baseService);
        serviceFactory.init('media');        
        return serviceFactory;

    }
]);