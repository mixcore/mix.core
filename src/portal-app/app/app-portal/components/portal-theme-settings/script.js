modules.component('portalThemeSettings', {
    templateUrl: '/app/app-portal/components/portal-theme-settings/view.html',
    bindings: {
        showLink: '='
    },
    controller: ['$rootScope','$scope','AppSettingsServices',
        function ($rootScope, $scope, appSettingsServices) {
            var ctrl = this;
            this.$onInit = function () {
                ctrl.portalThemeSettings = $rootScope.globalSettings.portalThemeSettings;
            }
            ctrl.applyThemeSettings = function () {
                $rootScope.globalSettings.portalThemeSettings = ctrl.portalThemeSettings;
            }
            ctrl.saveThemeSettings = async function () {
                var resp = await appSettingsServices.saveGlobalSettings('PortalThemeSettings', ctrl.portalThemeSettings);
                if (resp && resp.isSucceed) {
                    $rootScope.showMessage('success', 'success');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $scope.$apply();
                }
            }
        }]
});