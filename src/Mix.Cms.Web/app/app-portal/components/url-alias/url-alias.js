
modules.component('urlAlias', {
    templateUrl: '/app/app-portal/components/url-alias/url-alias.html',
    controller: ['$rootScope', '$scope', 'UrlAliasService',
        function ($rootScope, $scope, service) {
            var ctrl = this;
            ctrl.$onInit = function(){
                ctrl.updateUrl();
            }
            ctrl.updateUrl =  function(){
                ctrl.url = $rootScope.globalSettings.domain + '/'
                 + $rootScope.settings.lang + '/'  + ctrl.urlAlias.alias;
            }
            ctrl.remove = function () {
                if (ctrl.urlAlias.id > 0) {
                    $rootScope.showConfirm(ctrl, 'removeConfirmed', [ctrl.urlAlias.id], null, 'Remove', 'Are you sure');
                } else {
                    if (ctrl.removeCallback) {
                        ctrl.removeCallback({ index: ctrl.index });
                    }
                }
            };

            ctrl.removeConfirmed = async function (id) {
                $rootScope.isBusy = true;
                var result = await service.delete(id);
                if (result.isSucceed) {
                    if (ctrl.removeCallback) {
                        ctrl.removeCallback({ index: ctrl.index });
                    }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    $rootScope.showMessage('failed');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };

        }],
    bindings: {
        urlAlias: '=',
        index: '=',
        callback: '&',
        removeCallback: '&',
    }
});