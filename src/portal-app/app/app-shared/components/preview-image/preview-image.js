modules.component('previewImage', {
    templateUrl: '/app/app-shared/components/preview-image/preview-image.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', function ($rootScope, $scope) {
        var ctrl = this;
        ctrl.isImage= false;
        ctrl.init = function(){
            if(ctrl.imgSrc)
            {
                ctrl.isImage = ctrl.imgSrc.match(/([/|.|\w|\s|-])*\.(?:jpg|gif|png)/g);
            }
        }
        ctrl.showImage = async function (functionName, args, context) {
            $rootScope.preview('img', ctrl.imgSrc);
        };
    }],
    bindings: {
        imgHeight: '=',
        imgSrc: '='
    }
});