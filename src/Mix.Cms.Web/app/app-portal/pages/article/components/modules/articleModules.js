
app.component('articleModules', {
    templateUrl: '/app/app-portal/pages/article/components/modules/articleModules.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'ModuleDataService',
        function ($rootScope, $scope, ngAppSettings, moduleDataService) {
            var ctrl = this;
            ctrl.request = angular.copy(ngAppSettings.request);
            ctrl.translate = function (keyword) {
                return $rootScope.translate(keyword);
            };

            ctrl.removeData = function (id, moduleId) {
                $rootScope.showConfirm(ctrl, 'removeDataConfirmed', [id, moduleId], null, 'Remove Data', 'Are you sure');
            }
            ctrl.removeDataConfirmed = async function (id, moduleId) {
                $rootScope.isBusy = true;
                var result = await moduleDataService.removeModuleData(id);
                if (result.isSucceed) {
                    ctrl.loadModuleDatas(moduleId);
                }
                else {
                    $rootScope.showMessage('failed');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            ctrl.saveDataCallback = function(data){
                if(data){
                    ctrl.loadModuleDatas(data.moduleId);
                }
            }
            ctrl.loadModuleDatas = async function (id, pageIndex) {
                $rootScope.isBusy = true;
                $scope.dataColumns = [];
                var request = angular.copy(ngAppSettings.request);
                request.query = '?module_id=' + id + '&article_id=' + ctrl.article.id;
                if(pageIndex){
                    request.pageIndex = pageIndex;
                }
                var response = await moduleDataService.getModuleDatas(request);
                if (response.isSucceed) {
                    var nav = findObjectByKey(ctrl.article.moduleNavs, 'moduleId', id);
                    if (nav) {
                        nav.module.data = response.data;
                    }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    $rootScope.showErrors(response.errors);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };

            ctrl.updateDataInfos = async function (items) {
                $rootScope.isBusy = true;
                var resp = await moduleDataService.updateInfos(items);
                if (resp && resp.isSucceed) {
                    $scope.activedPage = resp.data;
                    $rootScope.showMessage('success', 'success');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
        }],
    bindings: {
        article: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});