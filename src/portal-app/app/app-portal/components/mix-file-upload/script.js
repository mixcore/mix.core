modules.component('mixFileUpload', {
    templateUrl: '/app/app-portal/components/mix-file-upload/view.html',
    bindings: {
        header: '=?',
        description: '=?',
        src: '=',
        srcUrl: '=',
        mediaFile: '=',
        formFile: '=',
        type: '=?',
        folder: '=?',
        auto: '=',
        accept: '=?',
        onDelete: '&?',
        onUpdate: '&?'
    },
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'MediaService', function ($rootScope, $scope, ngAppSettings, mediaService) {
        var ctrl = this;
        ctrl.isAdmin = $rootScope.isAdmin;
        var image_placeholder = '/assets/img/image_placeholder.jpg';
        ctrl.isImage = false;
        ctrl.mediaNavs = [];
        ctrl.$onInit = function () {
            ctrl.srcUrl = ctrl.srcUrl || image_placeholder;
            ctrl.isImage = ctrl.srcUrl.toLowerCase().match(/([/|.|\w|\s|-])*\.(?:jpg|jpeg|gif|png|svg)/g);
            ctrl.maxHeight = ctrl.maxHeight || '200px';
            ctrl.id = Math.floor(Math.random() * 100);
        };
        ctrl.$doCheck = function () {
            if (ctrl.src !== ctrl.srcUrl && ctrl.srcUrl != image_placeholder) {
                ctrl.src = ctrl.srcUrl;
                ctrl.isImage = ctrl.srcUrl.toLowerCase().match(/([/|.|\w|\s|-])*\.(?:jpg|jpeg|gif|png|svg)/g);
            }
        }.bind(ctrl);

        ctrl.selectFile = function (file, errFiles) {
            if (file !== undefined && file !== null) {
                ctrl.mediaFile.folder = ctrl.folder ? ctrl.folder : 'Media';
                ctrl.mediaFile.title = ctrl.title ? ctrl.title : '';
                ctrl.mediaFile.description = ctrl.description ? ctrl.description : '';
                ctrl.mediaFile.file = file;
                ctrl.formFile = file;
                if (ctrl.auto == 'true') {
                    ctrl.uploadFile(file);
                }
                else {
                    ctrl.srcUrl = null;
                    ctrl.src = null;
                    ctrl.isImage = file.name.match(/([/|.|\w|\s|-])*\.(?:jpg|jpeg|gif|png|svg)/g);
                    if (ctrl.isImage) {
                        ctrl.getBase64(file);
                    }
                }
            }
        };

        ctrl.uploadFile = async function (file) {
            if (file !== null) {
                $rootScope.isBusy = true;                
                if (ctrl.mediaFile) {                    
                    var response = await mediaService.uploadMedia(ctrl.mediaFile, file);
                    if (response.isSucceed) {
                        $scope.activedData = response.data;
                        $rootScope.isBusy = false;
                        $location.url($scope.referrerUrl);
                        $scope.$apply();
                    }
                    else {
                        $rootScope.showErrors(response.errors);
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                }
            }
            else {
                return null;
            }
        };
        ctrl.getBase64 = function (file) {
            if (file !== null) {
                $rootScope.isBusy = true;
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = function () {
                    if (ctrl.mediaFile) {
                        ctrl.mediaFile.fileName = file.name.substring(0, file.name.lastIndexOf('.'));
                        ctrl.mediaFile.extension = file.name.substring(file.name.lastIndexOf('.'));
                        ctrl.mediaFile.fileStream = reader.result;
                    }
                    ctrl.srcUrl = reader.result;
                    ctrl.isImage = ctrl.srcUrl.indexOf('data:image/') >= 0 || ctrl.srcUrl.toLowerCase().match(/([/|.|\w|\s|-])*\.(?:jpg|jpeg|gif|png|svg)/g);
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
        };

    }],

});