'use strict';
app.factory('LanguageServices', ['$http', '$rootScope', 'CommonService', function ($http, $rootScope, commonServices) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var languagesServiceFactory = {};

    var settings = $rootScope.globalSettings

    var _getLanguage = async function (id, type) {
        var apiUrl = '/' + settings.lang + '/language/';
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

    var _initLanguage = async function (type) {
        var apiUrl = '/' + settings.lang + '/language/';
        var req = {
            method: 'GET',
            url: apiUrl + 'init/' + type,
        };
        return await commonServices.getApiResult(req)
    };

    var _getLanguages = async function (request) {
        var apiUrl = '/' + settings.lang + '/language/';
        var req = {
            method: 'POST',
            url: apiUrl + 'list',
            data: JSON.stringify(request)
        };
        
        return await commonServices.getApiResult(req);
    };

    var _removeLanguage = async function (id) {
        var apiUrl = '/' + settings.lang + '/language/';
        var req = {
            method: 'GET',
            url: apiUrl + 'delete/' + id
        };
        return await commonServices.getApiResult(req)
    };

    var _saveLanguage = async function (language) {
        var apiUrl = '/' + settings.lang + '/language/';
        var req = {
            method: 'POST',
            url: apiUrl + 'save',
            data: JSON.stringify(language)
        };
        return await commonServices.getApiResult(req)
    };
    var _uploadLanguage = async function (languageFile) {
        //var container = $(this).parents('.model-language').first().find('.custom-file').first();
        if (languageFile.file !== undefined && languageFile.file !== null) {
            // Create FormData object
            var files = new FormData();

            // Looping over all files and add it to FormData object
            files.append(languageFile.file.name, languageFile.file);

            // Adding one more key to FormData object
            files.append('fileFolder', languageFile.folder); files.append('title', languageFile.title);
            files.append('description', languageFile.description);

            var req = {
                url: '/' + settings.lang + '/language/upload', //'/tts/UploadImage',
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

    languagesServiceFactory.getLanguage = _getLanguage;
    languagesServiceFactory.initLanguage = _initLanguage;
    languagesServiceFactory.getLanguages = _getLanguages;
    languagesServiceFactory.removeLanguage = _removeLanguage;
    languagesServiceFactory.saveLanguage = _saveLanguage;
    languagesServiceFactory.uploadLanguage = _uploadLanguage;
    return languagesServiceFactory;

}]);
