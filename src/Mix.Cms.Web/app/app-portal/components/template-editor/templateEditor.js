modules.component('templateEditor', {
    templateUrl: '/app-portal/components/template-editor/templateEditor.html',
    controller: ['$scope', '$rootScope', '$routeParams', 'ngAppSettings', 'GlobalSettingsService', 'TemplateService',
        function ($scope, $rootScope, $routeParams, ngAppSettings, globalSettingsService, service) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            var ctrl = this;
            
            ctrl.selectTemplate = function (template) {
                ctrl.template = template;
                $scope.$broadcast('updateContentCodeEditors', []);
            };
        }],
    bindings: {
        template: '=',
        templates: '='
    }
});
