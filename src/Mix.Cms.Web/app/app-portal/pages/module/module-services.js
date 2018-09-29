'use strict';
app.factory('ModuleServices', ['$http', '$rootScope', 'CommonService', function ($http, $rootScope, commonServices) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var modulesServiceFactory = {};

    var settings = $rootScope.globalSettings

    var _getModule = async function (id, type) {
        var apiUrl = '/' + settings.lang + '/module/';
        var url = apiUrl + 'details/' + type;
        if (id) {
            url += '/' + id;
        }
        var req = {
            method: 'GET',
            url: url
        };
        return await commonServices.getApiResult(req)
    };

    var _initModule = async function (type) {
        var apiUrl = '/' + settings.lang + '/module/';
        var req = {
            method: 'GET',
            url: apiUrl + 'init/' + type,
        };
        return await commonServices.getApiResult(req)
    };

    var _getModules = async function (request) {
        var apiUrl = '/' + settings.lang + '/module/';
        var req = {
            method: 'POST',
            url: apiUrl + 'list',
            data: JSON.stringify(request)
        };
        
        return await commonServices.getApiResult(req);
    };

    var _removeModule = async function (id) {
        var apiUrl = '/' + settings.lang + '/module/';
        var req = {
            method: 'GET',
            url: apiUrl + 'delete/' + id
        };
        return await commonServices.getApiResult(req)
    };

    var _saveModule = async function (module) {
        var apiUrl = '/' + settings.lang + '/module/';
        var req = {
            method: 'POST',
            url: apiUrl + 'save',
            data: JSON.stringify(module)
        };
        return await commonServices.getApiResult(req)
    };

    var _saveFields = async function (id, model, fieldName) {
        var apiUrl = '/' + settings.lang + '/module/';
        var field = [
            {
                PropertyName: fieldName,
                propertyValue: model[fieldName]
            }
        ]
        var req = {
            method: 'POST',
            url: apiUrl + 'save/'+ id,
            data: JSON.stringify(field)
        };
        return await commonServices.getApiResult(req)
    };

    modulesServiceFactory.getModule = _getModule;
    modulesServiceFactory.initModule = _initModule;
    modulesServiceFactory.getModules = _getModules;
    modulesServiceFactory.removeModule = _removeModule;
    modulesServiceFactory.saveModule = _saveModule;
    modulesServiceFactory.saveFields = _saveFields;
    return modulesServiceFactory;

}]);
