'use strict';
app.factory('PageService', ['$rootScope', 'CommonService', 'BaseService',
    function ($rootScope, commonServices, baseService) {

        var serviceFactory = Object.create(baseService);

        var _updateInfos = async function (pages) {

            var req = {
                method: 'POST',
                url: this.prefixUrl + '/update-infos',
                data: JSON.stringify(pages)
            };
            return await commonServices.getApiResult(req);
        };
        serviceFactory.modelName = 'page';
        serviceFactory.prefixUrl = '/' + serviceFactory.lang + '/page';
        serviceFactory.updateInfos = _updateInfos;
        return serviceFactory;

    }]);
