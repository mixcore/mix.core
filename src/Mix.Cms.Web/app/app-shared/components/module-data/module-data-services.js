'use strict';
app.factory('ModuleDataService', ['$http', '$rootScope', 'CommonService', function ($http, $rootScope, commonService) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var moduleDatasServiceFactory = {};

    var settings = $rootScope.globalSettings

    var _getModuleData = async function (moduleId, id, type) {
        var apiUrl = '/' + settings.lang + '/module-data/';
        var url = apiUrl + 'details/' + type;
        if (id) {
            url += '/' + moduleId + '/' + id;
        } else {
            url += '/' + moduleId;
        }
        var req = {
            method: 'GET',
            url: url
        };
        return await commonService.getApiResult(req)
    };


    var _getModuleDatas = async function (request) {
        var apiUrl = '/' + settings.lang + '/module-data/';
        var req = {
            method: 'POST',
            url: apiUrl + 'list',
            data: JSON.stringify(request)
        };
        
        return await commonService.getApiResult(req);
    };

    var _initModuleForm = async function (name) {
        if (!settings) {
            settings = await commonService.fillSettings();
        }
        var apiUrl = '/' + settings.lang + '/module-data/';
        var req = {
            method: 'GET',
            url: apiUrl + 'init-by-name/' + name,
        };
        
        return await commonService.getApiResult(req);
    };

    var _removeModuleData = async function (id) {
        var apiUrl = '/' + settings.lang + '/module-data/';
        var req = {
            method: 'GET',
            url: apiUrl + 'delete/' + id
        };
        return await commonService.getApiResult(req);
    };

    var _saveModuleData = async function (moduleData) {
        var apiUrl = '/' + settings.lang + '/module-data/';
        var req = {
            method: 'POST',
            url: apiUrl + 'save',
            data: JSON.stringify(moduleData)
        };
        return await commonService.getApiResult(req);
    };
    var _saveFields = async function (id, propertyName, propertyValue) {
        var apiUrl = '/' + settings.lang + '/module-data/';
        var field = [
            {
                propertyName: propertyName,
                propertyValue: propertyValue
            }
        ]
        var req = {
            method: 'POST',
            url: apiUrl + 'save/'+ id,
            data: JSON.stringify(field)
        };
        return await commonService.getApiResult(req)
    };
    moduleDatasServiceFactory.getModuleData = _getModuleData;
    moduleDatasServiceFactory.getModuleDatas = _getModuleDatas;
    moduleDatasServiceFactory.removeModuleData = _removeModuleData;
    moduleDatasServiceFactory.saveModuleData = _saveModuleData;
    moduleDatasServiceFactory.initModuleForm = _initModuleForm;
    moduleDatasServiceFactory.saveFields = _saveFields;
    return moduleDatasServiceFactory;
}]);
