'use strict';
app.factory('MediaService', ['$rootScope', 'CommonService', 'BaseService',
    function ($rootScope, commonService, baseService) {

        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('media');
        var _cloneMedia = async function (id) {
            var req = {
                method: 'GET',
                url: serviceFactory.prefixUrl + '/clone/' + id
            };
            return await commonService.getApiResult(req);
        };
        var _save = async function(objData, file){
            var url = this.prefixUrl + '/save';
            var fd = new FormData();
            var file =objData.mediaFile.file;
            objData.mediaFile.file = null;
            fd.append('model', angular.toJson(objData));
            fd.append('file', file);
            return await serviceFactory.ajaxSubmitForm(fd, url);
        }
        var _uploadMedia = async function (mediaFile, file) {
            //var container = $(this).parents('.model-media').first().find('.custom-file').first();
            if (mediaFile.file !== undefined && mediaFile.file !== null) {
                // Create FormData object
                var files = new FormData();

                // Looping over all files and add it to FormData object
                files.append(mediaFile.file.name, file);

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
        serviceFactory.save = _save;
        return serviceFactory;

    }]);
