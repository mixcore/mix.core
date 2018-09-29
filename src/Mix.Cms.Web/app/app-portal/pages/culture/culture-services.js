'use strict';
app.factory('CultureServices', ['$http', '$rootScope', 'CommonService', function ($http, $rootScope, commonServices) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var culturesServiceFactory = {};

    var _getCulture = async function (id, type) {
        var apiUrl = '/api/culture/';
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

    var _syncTemplates = async function (id) {
        var apiUrl = '/api/culture/';
        var url = apiUrl + 'sync/' + id;        
        var req = {
            method: 'GET',
            url: url
        };
        return await commonServices.getApiResult(req)
    };

    var _initCulture = async function (type) {
        var apiUrl = '/api/culture/';
        var req = {
            method: 'GET',
            url: apiUrl + 'init/' + type,
        };
        return await commonServices.getApiResult(req)
    };

    var _getCultures = async function (request) {
        var apiUrl = '/api/culture/';
        var req = {
            method: 'POST',
            url: apiUrl + 'list',
            data: JSON.stringify(request)
        };
        
        return await commonServices.getApiResult(req);
    };

    var _removeCulture = async function (id) {
        var apiUrl = '/api/culture/';
        var req = {
            method: 'GET',
            url: apiUrl + 'delete/' + id
        };
        return await commonServices.getApiResult(req)
    };

    var _saveCulture = async function (culture) {
        var apiUrl = '/api/culture/';
        var req = {
            method: 'POST',
            url: apiUrl + 'save',
            data: JSON.stringify(culture)
        };
        return await commonServices.getApiResult(req)
    };

    culturesServiceFactory.syncTemplates = _syncTemplates;
    culturesServiceFactory.getCulture = _getCulture;
    culturesServiceFactory.initCulture = _initCulture;
    culturesServiceFactory.getCultures = _getCultures;
    culturesServiceFactory.removeCulture = _removeCulture;
    culturesServiceFactory.saveCulture = _saveCulture;
    return culturesServiceFactory;

}]);
