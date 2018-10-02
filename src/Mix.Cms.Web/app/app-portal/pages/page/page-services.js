'use strict';
app.factory('PageService', ['$rootScope', 'CommonService', 'BaseService',
    function ($rootScope, commonServices, baseService) {

        var serviceFactory = Object.create(baseService);
        serviceFactory.init('page');
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
