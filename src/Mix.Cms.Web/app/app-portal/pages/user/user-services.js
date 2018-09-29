'use strict';
app.factory('UserServices', ['$http', 'CommonService', 'ngAppSettings'
    , function ($http, commonServices, ngAuthSettings) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var usersServiceFactory = {};
    var apiUrl = '/api/account/';
    
    var serviceBase = ngAuthSettings.serviceBase;
    
    var _getUserDemographicInfo = function () {
        var url = '/api/GetUserDemographicInfo';
        var req = {
            method: 'GET',
            url: serviceBase + url
        };

        return commonServices.getApiResult(req);
    };



    var _importUsers = function (strBase64) {
        var url = 'import-users';
        var req = {
            method: 'POST',
            url: apiUrl + url,
            data: JSON.stringify({ strBase64: strBase64 })
        };

        return commonServices.getApiResult(req);
    };

    var _getUsers = function (request) {

        var req = {
            method: 'POST',
            url: apiUrl + 'list',
            data: request
        };

        return commonServices.getApiResult(req);
    };

    var _getUser = async function (id, viewType) {
        var apiUrl = '/api/account/';
        var url = apiUrl + 'details/' + viewType;
        if (id) {
            url += '/' + id;
        }
        var req = {
            method: 'GET',
            url: url
        };
        return await commonServices.getApiResult(req)
    };

    var _updateRoleStatus = function (userInRole) {

        var req = {
            method: 'POST',
            url: serviceBase + '/api/account/user-in-role',
            data: JSON.stringify(userInRole)

        };

        return commonServices.getApiResult(req);
    };

    var _saveUser = async function (user) {
        var apiUrl = '/api/account/';
        var req = {
            method: 'POST',
            url: apiUrl + 'save',
            data: JSON.stringify(user)
        };
        return await commonServices.getApiResult(req)
    };

    var _register = async function (user) {
        var apiUrl = '/api/account/';
        var req = {
            method: 'POST',
            url: apiUrl + 'register',
            data: JSON.stringify(user)
        };
        return await commonServices.getApiResult(req)
    };

    var _removeUser = function (user) {
        var req = {
            method: 'POST',
            url: apiUrl + 'remove-user',
            data: JSON.stringify(user)
        };

        return commonServices.getApiResult(req);
    };

    usersServiceFactory.importUsers = _importUsers;
    usersServiceFactory.getUsers = _getUsers;
    usersServiceFactory.getUser = _getUser;
    usersServiceFactory.saveUser = _saveUser;
    usersServiceFactory.register = _register;
    usersServiceFactory.removeUser = _removeUser;
    usersServiceFactory.updateRoleStatus = _updateRoleStatus;
    usersServiceFactory.getUserDemographicInfo = _getUserDemographicInfo;
    return usersServiceFactory;

}]);
