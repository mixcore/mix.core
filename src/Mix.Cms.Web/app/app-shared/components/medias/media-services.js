'use strict';
app.factory('MediaService', ['$rootScope', 'CommonService', 'BaseService',
    function ($rootScope, commonService, baseService) {

        var serviceFactory = Object.create(baseService);
        serviceFactory.init('media');
        var _cloneMedia = async function (id) {
            var req = {
                method: 'GET',
                url: this.prefixUrl + '/clone/' + id
            };
            return await commonService.getApiResult(req);
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
                    url: this.prefixUrl + '/media/upload', //'/tts/UploadImage',
                    method: "POST",
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    },
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: files,
                };

                return await commonService.getApiResult(req);
            }
        };
        serviceFactory.cloneMedia = _cloneMedia;
        serviceFactory.uploadMedia = _uploadMedia;
        return serviceFactory;

    }]);
