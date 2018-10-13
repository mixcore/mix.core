'use strict';
app.factory('PortalPageServices', ['$http', '$rootScope', 'CommonService', function ($http, $rootScope, commonServices) {

    var pagesServiceFactory = {};

    var _getPage = async function (id, type) {
        var apiUrl = '/portal-page/';
        var url = apiUrl + 'details/' + type;
        if (id) {
            url += '/' + id;
        }
        var req = {
            method: 'GET',
            url: url
        };
        return await commonServices.getApiResult(req);
    };

    var _initPage = async function (type) {
        var apiUrl = '/portal-page/';
        var req = {
            method: 'GET',
            url: apiUrl + 'init/' + type
        };
        return await commonServices.getApiResult(req);
    };

    var _getPages = async function (request) {
        var apiUrl = '/portal-page/';
        var req = {
            method: 'POST',
            url: apiUrl + 'list/0',
            data: JSON.stringify(request)
        };
        
        return await commonServices.getApiResult(req);
    };

    var _removePage = async function (id) {
        var apiUrl = '/portal-page/';
        var req = {
            method: 'GET',
            url: apiUrl + 'delete/' + id
        };
        return await commonServices.getApiResult(req);
    };

    var _savePage = async function (page) {
        var apiUrl = '/portal-page/';
        var req = {
            method: 'POST',
            url: apiUrl + 'save',
            data: JSON.stringify(page)
        };
        return await commonServices.getApiResult(req);
    };

    var _updateInfos = async function (pages) {
        var apiUrl = '/portal-page/';
        var req = {
            method: 'POST',
            url: apiUrl + 'update-infos',
            data: JSON.stringify(pages)
        };
        return await commonServices.getApiResult(req);
    };

    var _updateChildInfos = async function (pages) {
        var apiUrl = '/portal-page/';
        var req = {
            method: 'POST',
            url: apiUrl + 'update-child-infos',
            data: JSON.stringify(pages)
        };
        return await commonServices.getApiResult(req);
    };

    pagesServiceFactory.getPage = _getPage;
    pagesServiceFactory.initPage = _initPage;
    pagesServiceFactory.getPages = _getPages;
    pagesServiceFactory.removePage = _removePage;
    pagesServiceFactory.savePage = _savePage;
    pagesServiceFactory.updateInfos = _updateInfos;
    pagesServiceFactory.updateChildInfos = _updateChildInfos;
    return pagesServiceFactory;

}]);
