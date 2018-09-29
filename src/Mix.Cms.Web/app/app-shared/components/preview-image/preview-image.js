modules.component('previewImage', {
    templateUrl: '/app-shared/components/preview-image/preview-image.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', function ($rootScope, $scope) {
        var ctrl = this;
        ctrl.showImage = async function (functionName, args, context) {
            $rootScope.preview('img', ctrl.imgSrc);
        };
    }],
    bindings: {
        imgHeight: '=',
        imgSrc: '='
    }
});