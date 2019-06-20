
modules.component('modulePreview', {
    templateUrl: '/app/app-shared/components/module-preview/view.html',
    controller: ['$scope', '$rootScope', 'SharedModuleDataService',
        function ($scope, $rootScope, moduleDataService) {
            var ctrl = this;
            $rootScope.isBusy = false;
            ctrl.previousContentId = undefined;

            this.$onInit = () => { ctrl.previousContentId = angular.copy(ctrl.contentId) };

            this.$doCheck = () => {
                if (ctrl.contentId !== ctrl.previousContentId) {
                    ctrl.loadModuleData();
                    ctrl.previousContentId = ctrl.contentId;
                }
            };
            
            ctrl.loadModuleData = async function () {
                $rootScope.isBusy = true;                
                var response = await moduleDataService.getModuleData(ctrl.moduleId, ctrl.contentId, 'portal');
                if (response.isSucceed) {
                    ctrl.data = response.data;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    $rootScope.showErrors(response.errors);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
        }],
    bindings: {
        moduleId: '=',
        contentId: '='
    }
});
