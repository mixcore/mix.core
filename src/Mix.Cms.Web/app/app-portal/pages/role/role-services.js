'use strict';
app.factory('RoleService', ['BaseService','CommonService', function (baseService, commonService) {
    var serviceFactory = Object.create(baseService);
    serviceFactory.init('role', true);

    var _getPermissions = async function () {
        var req = {
            method: 'GET',
            url: this.prefixUrl + '/permissions'
        };
        return await commonService.getApiResult(req);
    };

    var _updatePermission = async function (permission) {
        var req = {
            method: 'POST',
            url: this.prefixUrl + '/update-permission',
            data: JSON.stringify(permission)
        };
        return await commonService.getApiResult(req);
    };
    var _createRole = function (name) {
        var req = {
            method: 'POST',
            url: this.prefixUrl + '/create',
            data: JSON.stringify(name)
        };

        return commonService.getApiResult(req);
    };
    serviceFactory.createRole = _createRole;
    serviceFactory.getPermissions = _getPermissions;
    serviceFactory.updatePermission = _updatePermission;
    return serviceFactory;

}]);
