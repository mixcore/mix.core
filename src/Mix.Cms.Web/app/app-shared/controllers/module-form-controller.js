'use strict';
app.controller('ModuleFormController', [
    '$scope', 'ModuleDataService',
    function ($scope, moduleDataService) {
        $scope.initModuleForm = async function (name) {
            var resp = null;
            if (!$rootScope.isInit) {
                setTimeout(function () { $scope.initModuleForm(name); }, 500);
            } else {
                resp = await moduleDataService.initModuleForm(name);
                if (resp && resp.isSucceed) {
                    $scope.data = resp.data;
                    $scope.data.articleId = $scope.articleId;
                    $scope.data.productId = $scope.productId;
                    $scope.data.categoryId = $scope.categoryId;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                    //$scope.initEditor();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $scope.$apply();
                }
            }
        };

        $scope.loadModuleData = async function (id) {
            $rootScope.isBusy = true;
            var response = await moduleDataService.getModuleData($scope.moduleId, $scope.d, 'portal');
            if (response.isSucceed) {
                $scope.data = response.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.saveModuleData = async function () {
            var form = $('#module-' + $scope.data.moduleId);
            $.each($scope.data.dataProperties, function (i, e) {
                switch (e.dataType) {
                    case 5:
                        e.value = $(form).find('.' + e.name).val();
                        break;
                    default:
                        e.value = e.value ? e.value.toString() : null;
                        break;
                }
            });
            var resp = await moduleDataService.saveModuleData($scope.data);
            if (resp && resp.isSucceed) {
                $scope.data = resp.data;
                $scope.initModuleForm();
                $rootScope.showMessage('success', 'success');
                if ($scope.saveCallback) {
                    $scope.saveCallback({ data: $scope.data });
                }
                else {
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
    }]);