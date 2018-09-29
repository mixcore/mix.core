modules.component('previewPopup', {
    templateUrl: '/app-shared/components/preview-popup/preview-popup.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', function ($rootScope, $scope) {
        var ctrl = this;
        ctrl.preview = async function () {
            
        };
    }],
    bindings: {
        previewObject: '='
    }
});