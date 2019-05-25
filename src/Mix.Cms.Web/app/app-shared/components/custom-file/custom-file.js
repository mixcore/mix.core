
//modules.controller('ImageController', );
modules.component('customFile', {
    templateUrl: '/app/app-shared/components/custom-file/custom-file.html',
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
                    var getMedia = await mediaService.getSingle(['portal']);
                    if (getMedia.isSucceed) {
                        var index = reader.result.indexOf(',') + 1;
                        var base64 = reader.result.substring(index);
                        ctrl.data.mediaFile.fileName = file.name.substring(0, file.name.lastIndexOf('.'));
                        ctrl.data.mediaFile.extension = file.name.substring(file.name.lastIndexOf('.'));
                        ctrl.data.mediaFile.fileStream = reader.result;
                        var media = getMedia.data;
                        media.data.mediaFile = ctrl.data.mediaFile;
                        var resp = await mediaService.save(media);
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
            if (file !== null && ctrl.data.mediaFile) {
                $rootScope.isBusy = true;
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = function () {
                    var index = reader.result.indexOf(',') + 1;
                    var base64 = reader.result.substring(index);
                    ctrl.data.mediaFile.fileName = file.name.substring(0, file.name.lastIndexOf('.'));
                    ctrl.data.mediaFile.extension = file.name.substring(file.name.lastIndexOf('.'));
                    ctrl.data.mediaFile.fileStream = reader.result;
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
        data: '=',
        type: '=',
        folder: '=',
        auto: '=',
        onDelete: '&',
        save: '&',
    }
});