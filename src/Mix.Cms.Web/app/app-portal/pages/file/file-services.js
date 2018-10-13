'use strict';
app.factory('FileServices', ['$http', '$rootScope', 'CommonService', function ($http, $rootScope, commonServices) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var filesServiceFactory = {};

    var settings = $rootScope.globalSettings

    var _getFile = async function (folder, filename) {
        var apiUrl = '/file/';
        var url = apiUrl + 'details?folder=' + folder + '&filename=' + filename;        
        var req = {
            method: 'GET',
            url: url
        };
        return await commonServices.getApiResult(req)
    };

    var _initFile = async function (type) {
        var apiUrl = '/file/';
        var req = {
            method: 'GET',
            url: apiUrl + 'init/' + type,
        };
        return await commonServices.getApiResult(req)
    };

    var _getFiles = async function (request) {
        var apiUrl = '/file/';
        var req = {
            method: 'POST',
            url: apiUrl + 'list',
            data: JSON.stringify(request)
        };
        
        return await commonServices.getApiResult(req);
    };

    var _removeFile = async function (id) {
        var apiUrl = '/file/';
        var req = {
            method: 'GET',
            url: apiUrl + 'delete/' + id
        };
        return await commonServices.getApiResult(req)
    };

    var _saveFile = async function (file) {
        var apiUrl = '/file/';
        var req = {
            method: 'POST',
            url: apiUrl + 'save',
            data: JSON.stringify(file)
        };
        return await commonServices.getApiResult(req)
    };

    filesServiceFactory.getFile = _getFile;
    filesServiceFactory.initFile = _initFile;
    filesServiceFactory.getFiles = _getFiles;
    filesServiceFactory.removeFile = _removeFile;
    filesServiceFactory.saveFile = _saveFile;
    return filesServiceFactory;

}]);
