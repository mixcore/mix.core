'use strict';
app.factory('RoleService', ['BaseService','CommonService', function (baseService, commonService) {
    var serviceFactory = Object.create(baseService);
    serviceFactory.init('role', true);

    var _getPermissions = async function () {
        var apiUrl = '/role/';
        var req = {
            method: 'GET',
            url: apiUrl + 'permissions'
        };
        return await commonService.getApiResult(req);
    };

    var _updatePermission = async function (permission) {
        var apiUrl = '/role/';
        var req = {
            method: 'POST',
            url: apiUrl + 'update-permission',
            data: JSON.stringify(permission)
        };
        return await commonService.getApiResult(req);
    };
    var _createRole = function (name) {
        var req = {
            method: 'POST',
            url: apiUrl + 'create',
            data: JSON.stringify(name)
        };

        return commonServices.getApiResult(req);
    };
    serviceFactory.createRole = _createRole;
    serviceFactory.getPermissions = _getPermissions;
    serviceFactory.updatePermission = _updatePermission;
    return serviceFactory;

}]);
