
//modules.controller('ImageController', );
modules.component('customFile', {
    templateUrl: '/app-shared/components/custom-file/custom-file.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'MediaService', function PortalTemplateController($rootScope, $scope, mediaService) {
        var ctrl = this;
        ctrl.media = null;
        ctrl.init = function () {
            ctrl.id = Math.random();
        };
        ctrl.selectFile = function (file, errFiles) {
            if (file !== undefined && file !== null) {
                ctrl.getBase64(file);
            }
        };

        ctrl.uploadFile = async function (file) {
            if (file !== null) {
                $rootScope.isBusy = true;
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = async function () {
                    var getMedia = await mediaService.getMedia(null, 'portal');
                    if (getMedia.isSucceed) {
                        var index = reader.result.indexOf(',') + 1;
                        var base64 = reader.result.substring(index);
                        ctrl.mediaFile.fileName = file.name.substring(0, file.name.lastIndexOf('.'));
                        ctrl.mediaFile.extension = file.name.substring(file.name.lastIndexOf('.'));
                        ctrl.mediaFile.fileStream = reader.result;
                        var media = getMedia.data;
                        media.mediaFile = ctrl.mediaFile;
                        var resp = await mediaService.saveMedia(media);
                        if (resp && resp.isSucceed) {
                            ctrl.src = resp.data.fullPath;
                            ctrl.srcUrl = resp.data.fullPath;
                            $rootScope.isBusy = false;
                            $scope.$apply();
                        }
                        else {
                            if (resp) { $rootScope.showErrors(resp.errors); }
                            $rootScope.isBusy = false;
                            $scope.$apply();
                        }
                    }

                };
                reader.onerror = function (error) {

                };
            }
            else {
                return null;
            }

        };
        ctrl.getBase64 = function (file) {
            if (file !== null && ctrl.postedFile) {
                $rootScope.isBusy = true;
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = function () {
                    var index = reader.result.indexOf(',') + 1;
                    var base64 = reader.result.substring(index);
                    ctrl.postedFile.fileName = file.name.substring(0, file.name.lastIndexOf('.'));
                    ctrl.postedFile.extension = file.name.substring(file.name.lastIndexOf('.'));
                    ctrl.postedFile.fileStream = reader.result;
                    ctrl.srcUrl = reader.result;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                };
                reader.onerror = function (error) {
                    $rootScope.isBusy = false;
                    $rootScope.showErrors([error]);
                };
            }
            else {
                return null;
            }
        };

    }],
    bindings: {
        header: '=',
        title: '=',
        description: '=',
        src: '=',
        srcUrl: '=',
        postedFile: '=',
        type: '=',
        folder: '=',
        auto: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});