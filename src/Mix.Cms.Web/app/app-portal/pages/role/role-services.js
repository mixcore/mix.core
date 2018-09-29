'use strict';
app.factory('RoleServices', ['$http', 'CommonService', function ($http, commonServices) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var rolesServiceFactory = {};
    var apiUrl = '/api/role/';
    
    var _getRoles = function (request) {

        var req = {
            method: 'POST',
            url: apiUrl + 'list',
            data: request
        };

        return commonServices.getApiResult(req);
    };

    var _getRole = async function (id, viewType) {
        var apiUrl = '/api/role/';
        var url = apiUrl + 'details/' + viewType;
        if (id) {
            url += '/' + id;
        }
        var req = {
            method: 'GET',
            url: url
        };
        return await commonServices.getApiResult(req);
    };

    var _getPermissions = async function () {
        var apiUrl = '/api/role/';
        var req = {
            method: 'GET',
            url: apiUrl + 'permissions'
        };
        return await commonServices.getApiResult(req);
    };

    var _saveRole = async function (role) {
        var apiUrl = '/api/role/';
        var req = {
            method: 'POST',
            url: apiUrl + 'save',
            data: JSON.stringify(role)
        };
        return await commonServices.getApiResult(req);
    };

    var _updatePermission = async function (permission) {
        var apiUrl = '/api/role/';
        var req = {
            method: 'POST',
            url: apiUrl + 'update-permission',
            data: JSON.stringify(permission)
        };
        return await commonServices.getApiResult(req);
    };

    var _removeRole = function (role) {
        var req = {
            method: 'POST',
            url: apiUrl + 'delete',
            data: JSON.stringify(role)
        };

        return commonServices.getApiResult(req);
    };

    var _createRole = function (name) {
        var req = {
            method: 'POST',
            url: apiUrl + 'create',
            data: JSON.stringify(name)
        };

        return commonServices.getApiResult(req);
    };

    rolesServiceFactory.getRoles = _getRoles;
    rolesServiceFactory.getPermissions = _getPermissions;
    rolesServiceFactory.getRole = _getRole;
    rolesServiceFactory.updatePermission = _updatePermission;
    rolesServiceFactory.saveRole = _saveRole;
    rolesServiceFactory.createRole = _createRole;
    rolesServiceFactory.removeRole = _removeRole;
    return rolesServiceFactory;

}]);
