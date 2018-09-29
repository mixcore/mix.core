'use strict';
app.factory('MediaServices', ['$http', '$rootScope', 'CommonService', function ($http, $rootScope, commonServices) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var mediasServiceFactory = {};

    var settings = $rootScope.globalSettings

    var _getMedia = async function (id, type) {
        var apiUrl = settings.lang + '/media/';
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

    var _initMedia = async function (type) {
        var apiUrl = '/' + settings.lang + '/media/';
        var req = {
            method: 'GET',
            url: apiUrl + 'init/' + type,
        };
        return await commonServices.getApiResult(req)
    };

    var _getMedias = async function (request) {
        var apiUrl = '/' + settings.lang + '/media/';
        var req = {
            method: 'POST',
            url: apiUrl + 'list',
            data: JSON.stringify(request)
        };
        
        return await commonServices.getApiResult(req);
    };

    var _removeMedia = async function (id) {
        var apiUrl = '/' + settings.lang + '/media/';
        var req = {
            method: 'GET',
            url: apiUrl + 'delete/' + id
        };
        return await commonServices.getApiResult(req)
    };

    var _saveMedia = async function (media) {
        var apiUrl = '/' + settings.lang + '/media/';
        var req = {
            method: 'POST',
            url: apiUrl + 'save',
            data: JSON.stringify(media)
        };
        return await commonServices.getApiResult(req)
    };
    var _uploadMedia = async function (mediaFile) {
        //var container = $(this).parents('.model-media').first().find('.custom-file').first();
        if (mediaFile.file !== undefined && mediaFile.file !== null) {
            // Create FormData object
            var files = new FormData();

            // Looping over all files and add it to FormData object
            files.append(mediaFile.file.name, mediaFile.file);

            // Adding one more key to FormData object
            files.append('fileFolder', mediaFile.folder); files.append('title', mediaFile.title);
            files.append('description', mediaFile.description);

            var req = {
                url: '/' + settings.lang + '/media/upload', //'/tts/UploadImage',
                type: "POST",
                headers: {
                    'Content-Type': 'multipart/form-data'
                },
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: files,
            };

            return await commonServices.getApiResult(req)
        }
    };

    mediasServiceFactory.getMedia = _getMedia;
    mediasServiceFactory.initMedia = _initMedia;
    mediasServiceFactory.getMedias = _getMedias;
    mediasServiceFactory.removeMedia = _removeMedia;
    mediasServiceFactory.saveMedia = _saveMedia;
    mediasServiceFactory.uploadMedia = _uploadMedia;
    return mediasServiceFactory;

}]);
