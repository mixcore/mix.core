'use strict';
app.factory('PageService', ['$rootScope', 'CommonService', 'BaseService',
    function ($rootScope, commonServices, baseService) {

        baseService.init('page');
        var serviceFactory = baseService;//Object.create(baseService);

        var _updateInfos = async function (pages) {

            var req = {
                method: 'POST',
                url: this.prefixUrl + '/update-infos',
                data: JSON.stringify(pages)
            };
            return await commonServices.getApiResult(req);
        };
        serviceFactory.updateInfos = _updateInfos;
        return serviceFactory;

    }]);
