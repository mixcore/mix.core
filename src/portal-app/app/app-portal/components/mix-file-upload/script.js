modules.component('mixFileUpload', {
    templateUrl: '/app/app-portal/components/mix-file-upload/view.html',
    bindings: {
        folder: '=?',
        accept: '=?',
        onFail: '&?',
        onSuccess: '&?'
    },
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'FileServices', function ($rootScope, $scope, ngAppSettings, fileService) {
        var ctrl = this;
        ctrl.isAdmin = $rootScope.isAdmin;
        ctrl.mediaNavs = [];
        ctrl.$onInit = function () {
            ctrl.id = Math.floor(Math.random() * 100);
        };
        ctrl.selectFile = function (file, errFiles) {
            if (file !== undefined && file !== null) {
                ctrl.file = file;
            }
        };

        ctrl.uploadFile = async function () {
            if (ctrl.file) {
                $rootScope.isBusy = true;
                var response = await fileService.uploadFile(ctrl.file, ctrl.folder);
                if (response.isSucceed) {
                    if(ctrl.onSuccess){
                        ctrl.onSuccess();
                    }
                    $rootScope.showMessage('success', 'success');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    $rootScope.showErrors(response.errors);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            }
            else {
                $rootScope.showErrors(['Please choose file']);
            }
        };

    }],

});