'use strict';
app.factory('PagePostService', ['$rootScope', 'CommonService', 'BaseService',
    function ($rootScope, commonService, baseService) {

        var serviceFactory = Object.create(baseService);
        serviceFactory.init('page-post');
       var _updateInfos = async function (pages) {

            var req = {
                method: 'POST',
                url: this.prefixUrl + '/update-infos',
                data: JSON.stringify(pages)
            };
            return await commonService.getApiResult(req);
        };
        serviceFactory.updateInfos = _updateInfos;
        return serviceFactory;

    }]);
