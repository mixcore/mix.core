
app.component('postModules', {
    templateUrl: '/app/app-portal/pages/post/components/modules/view.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'SharedModuleDataService',
        function ($rootScope, $scope, ngAppSettings, moduleDataService) {
            var ctrl = this;
            ctrl.request = angular.copy(ngAppSettings.request);
            ctrl.translate = function (keyword, wrap, defaultValue) {
                return $rootScope.translate(keyword,wrap, defaultValue);
            };

            ctrl.removeData = function (id, moduleId) {
                $rootScope.showConfirm(ctrl, 'removeDataConfirmed', [id, moduleId], null, 'Remove Data', 'Deleted data will not able to recover, are you sure you want to delete this item?');
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
                request.query = '?module_id=' + id + '&post_id=' + ctrl.post.id;
                if(pageIndex){
                    request.pageIndex = pageIndex;
                }
                var response = await moduleDataService.getModuleDatas(request);
                if (response.isSucceed) {
                    var nav = $rootScope.findObjectByKey(ctrl.post.moduleNavs, 'moduleId', id);
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
        post: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});