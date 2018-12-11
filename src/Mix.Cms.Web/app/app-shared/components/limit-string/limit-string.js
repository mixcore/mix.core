
modules.component('limitString', {
    templateUrl: '/app/app-shared/components/limit-string/limit-string.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings',
        function ($rootScope, $scope, ngAppSettings) {
            var ctrl = this;
            ctrl.shortenString = '';
            var previousContentId = undefined;

            this.$onInit = () => { previousContentId = angular.copy(ctrl.contentId) };

            this.$doCheck = () => {
                if (!angular.equals(ctrl.contentId, previousContentId)) {
                    ctrl.loadModuleData();
                    previousContentId = angular.copy(ctrl.contentId);
                }
            };
            ctrl.shortString = function () {
                var data = decodeURIComponent(ctrl.content);
                if (ctrl.max < data.length) {
                    ctrl.shortenString = data.replace(/[+]/g, ' ').substr(0, ctrl.max) + ' ...';
                }
                else {
                    ctrl.shortenString = data.replace(/[+]/g, ' ');
                }
            }
            ctrl.view = function () {
                var obj = {
                    moduleId: ctrl.moduleId,
                    id: ctrl.contentId
                };
                $rootScope.preview('module-data', obj, null, 'modal-lg');
            }
        }],
    bindings: {
        content: '=',
        max: '=',
        moduleId: '=',
        contentId: '='
    }
});