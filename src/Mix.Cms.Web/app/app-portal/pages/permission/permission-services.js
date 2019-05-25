'use strict';
app.factory('PermissionService', ['BaseService','CommonService', function (baseService, commonService) {

    var serviceFactory = Object.create(baseService);
    serviceFactory.init('permission', true);
    var _updateInfos = async function (pages) {
        var req = {
            method: 'POST',
            url: this.prefixUrl + '/update-infos',
            data: JSON.stringify(pages)
        };
        return await commonService.getApiResult(req);
    };

    var _updateChildInfos = async function (pages) {
        var req = {
            method: 'POST',
            url: this.prefixUrl + '/update-child-infos',
            data: JSON.stringify(pages)
        };
        return await commonService.getApiResult(req);
    };

    serviceFactory.updateInfos = _updateInfos;
    serviceFactory.updateChildInfos = _updateChildInfos;
    return serviceFactory;

}]);
