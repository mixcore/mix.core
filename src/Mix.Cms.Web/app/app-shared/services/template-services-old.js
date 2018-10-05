'use strict';
app.factory('TemplateServicesbk', ['$http', '$rootScope', 'CommonService', function ($http, $rootScope, commonServices) {
    var serviceFactory = Object.create(baseService);
    serviceFactory.init('template');

    var templateServiceFactory = {};

    var settings = $rootScope.globalSettings

    var _getTemplate = async function (themeId, folderType, id, type) {
        var apiUrl = '/' + settings.lang + '/template/details/' + type + '/' + themeId + '/' + folderType;
        if (id) {
            apiUrl += '/' + id;
        }
        var req = {
            method: 'GET',
            url: apiUrl
        };
        return await commonServices.getApiResult(req)
    };


    var _getTemplates = async function (request, folderType, themeId) {
        var apiUrl = '/' + settings.lang + '/template/list/' + (themeId || settings.themeId);
        var req = {
            method: 'POST',
            url: apiUrl,
            data: JSON.stringify(request)
        };

        return await commonServices.getApiResult(req);
    };

    var _initModuleForm = async function (name) {
        var apiUrl = '/' + settings.lang + '/template/';
        var req = {
            method: 'GET',
            url: apiUrl + 'init/' + name,
        };

        return await commonServices.getApiResult(req);
    };

    var _removeTemplate = async function (id) {
        var apiUrl = '/' + settings.lang + '/template/';
        var req = {
            method: 'GET',
            url: apiUrl + 'delete/' + id
        };
        return await commonServices.getApiResult(req)
    };

    var _saveTemplate = async function (template) {
        var apiUrl = '/' + settings.lang + '/template/';
        var req = {
            method: 'POST',
            url: apiUrl + 'save',
            data: JSON.stringify(template)
        };
        return await commonServices.getApiResult(req)
    };

    templateServiceFactory.getTemplate = _getTemplate;
    templateServiceFactory.getTemplates = _getTemplates;
    templateServiceFactory.removeTemplate = _removeTemplate;
    templateServiceFactory.saveTemplate = _saveTemplate;
    templateServiceFactory.initModuleForm = _initModuleForm;
    return templateServiceFactory;
}]);
