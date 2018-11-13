modules.component('templateEditor', {
    templateUrl: '/app/app-portal/components/template-editor/templateEditor.html',
    controller: ['$scope', '$rootScope', '$routeParams', 'ngAppSettings', 'GlobalSettingsService', 'TemplateService',
        function ($scope, $rootScope, $routeParams, ngAppSettings, globalSettingsService, service) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            var ctrl = this;
            
            ctrl.selectTemplate = function (template) {
                ctrl.template = template;
                $scope.$broadcast('updateContentCodeEditors', []);
            };
            ctrl.init = async function(){
                if(ctrl.folderType && !ctrl.folderType){
                    var themeId = globalSettingsService.get('themeId');
                    var resp = await service.getSingle(['portal', themeId, ctrl.folderType]);
                    if (resp && resp.isSucceed) {
                        resp.data.fileName = 'new';
                        ctrl.templates.splice(0,0,resp.data);
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                    else {
                        if (resp) { $rootScope.showErrors(resp.errors);$rootScope.isBusy = false;$scope.$apply(); }
                    }
                    
                }
            }
        }],
    bindings: {
        template: '=',
        templates: '=',
        folderType: '='
    }
});
