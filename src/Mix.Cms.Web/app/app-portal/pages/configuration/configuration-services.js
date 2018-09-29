'use strict';
app.factory('ConfigurationServices', ['$http', '$rootScope', 'CommonService', function ($http, $rootScope, commonServices) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var configurationsServiceFactory = {};

    var settings = $rootScope.globalSettings

    var _getConfiguration = async function (id, type) {
        var apiUrl = '/' + settings.lang + '/configuration/';
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

    var _initConfiguration = async function (type) {
        var apiUrl = '/' + settings.lang + '/configuration/';
        var req = {
            method: 'GET',
            url: apiUrl + 'init/' + type,
        };
        return await commonServices.getApiResult(req)
    };

    var _getConfigurations = async function (request) {
        var apiUrl = '/' + settings.lang + '/configuration/';
        var req = {
            method: 'POST',
            url: apiUrl + 'list',
            data: JSON.stringify(request)
        };
        
        return await commonServices.getApiResult(req);
    };

    var _removeConfiguration = async function (id) {
        var apiUrl = '/' + settings.lang + '/configuration/';
        var req = {
            method: 'GET',
            url: apiUrl + 'delete/' + id
        };
        return await commonServices.getApiResult(req)
    };

    var _saveConfiguration = async function (configuration) {
        var apiUrl = '/' + settings.lang + '/configuration/';
        var req = {
            method: 'POST',
            url: apiUrl + 'save',
            data: JSON.stringify(configuration)
        };
        return await commonServices.getApiResult(req)
    };
    var _uploadConfiguration = async function (configurationFile) {
        //var container = $(this).parents('.model-configuration').first().find('.custom-file').first();
        if (configurationFile.file !== undefined && configurationFile.file !== null) {
            // Create FormData object
            var files = new FormData();

            // Looping over all files and add it to FormData object
            files.append(configurationFile.file.name, configurationFile.file);

            // Adding one more key to FormData object
            files.append('fileFolder', configurationFile.folder); files.append('title', configurationFile.title);
            files.append('description', configurationFile.description);

            var req = {
                url: '/' + settings.lang + '/configuration/upload', //'/api/tts/UploadImage',
                type: "POST",
                headers: {
                },
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: files,
            };

            return await commonServices.getApiResult(req)
        }
    };

    configurationsServiceFactory.getConfiguration = _getConfiguration;
    configurationsServiceFactory.initConfiguration = _initConfiguration;
    configurationsServiceFactory.getConfigurations = _getConfigurations;
    configurationsServiceFactory.removeConfiguration = _removeConfiguration;
    configurationsServiceFactory.saveConfiguration = _saveConfiguration;
    configurationsServiceFactory.uploadConfiguration = _uploadConfiguration;
    return configurationsServiceFactory;

}]);
