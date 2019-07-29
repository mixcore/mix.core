
modules.component('customImage', {
    templateUrl: '/app/app-portal/components/custom-image/custom-image.html',
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
    },
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'MediaService', function ($rootScope, $scope, ngAppSettings, mediaService) {
        var ctrl = this;
        ctrl.isAdmin = $rootScope.isAdmin;
        var image_placeholder = '/assets/img/image_placeholder.jpg';
        ctrl.isImage = false;
        ctrl.mediaNavs = [];
        ctrl.init = function () {
            ctrl.srcUrl = ctrl.srcUrl || image_placeholder;
            ctrl.isImage = ctrl.srcUrl.toLowerCase().match(/([/|.|\w|\s|-])*\.(?:jpg|jpeg|gif|png|svg)/g);
            ctrl.maxHeight = ctrl.maxHeight|| '200px';
            ctrl.id = Math.floor(Math.random() * 100);
        };
        
        ctrl.mediaFile = {
            file: null,
            fullPath: '',
            folder: ctrl.folder,
            title: ctrl.title,
            description: ctrl.description
        };
        ctrl.media = null;
        ctrl.$doCheck = function () {
            if (ctrl.src !== ctrl.srcUrl && ctrl.srcUrl != image_placeholder) {
                ctrl.src = ctrl.srcUrl;
            }
        }.bind(ctrl);
        // ctrl.updateSrc = function () {
        //     alert('asdfa');
        //     if (ctrl.src !== ctrl.srcUrl && ctrl.srcUrl != image_placeholder) {
        //         ctrl.src = ctrl.srcUrl;
        //     }
        // };
        ctrl.selectFile = function (file, errFiles) {
            if (file !== undefined && file !== null) {
                ctrl.mediaFile.folder = ctrl.folder ? ctrl.folder : 'Media';
                ctrl.mediaFile.title = ctrl.title ? ctrl.title : '';
                ctrl.mediaFile.description = ctrl.description ? ctrl.description : '';
                ctrl.mediaFile.file = file;

                if (ctrl.auto) {
                    ctrl.uploadFile(file);
                }
                else {
                    ctrl.getBase64(file);
                }
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
                        ctrl.mediaFile.fileName = file.name.substring(0, file.name.lastIndexOf('.'));
                        ctrl.mediaFile.extension = file.name.substring(file.name.lastIndexOf('.'));
                        ctrl.mediaFile.fileStream = reader.result;
                        var media = getMedia.data;
                        media.title = ctrl.title;
                        media.description = ctrl.description;
                        media.mediaFile = ctrl.mediaFile;
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

        }
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
                    ctrl.src = reader.result;
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
        }

    }],
    
});