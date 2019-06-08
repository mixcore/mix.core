'use strict';
app.controller('TemplateController', ['$scope', '$rootScope', '$routeParams', '$location', 'ngAppSettings', 'TemplateService',
    function ($scope, $rootScope, $routeParams, $location, ngAppSettings, service) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
        $scope.folderTypes = [
            'Masters',
            'Layouts',
            'Pages',
            'Modules',
            'Forms',
            'Edms',
            'Products',
            'Articles',
            'Widgets',
        ];
        $scope.activedPane = null;
        $scope.selectPane=function(pane){
            $scope.activedPane = pane;
        }
        $scope.loadFolder = function (d) {
            $location.url('/portal/template/list/' + $routeParams.themeId + '?folderType=' + encodeURIComponent(d));
        }
        $scope.loadParams = async function () {
            $rootScope.isBusy = true;
            $scope.folderType = $routeParams.folderType;// ? $routeParams.folderType : 'Masters';
            $scope.themeId = $routeParams.themeId;
        }
        $scope.getSingle = async function () {
            $rootScope.isBusy = true;
            var id = $routeParams.id;
            $scope.folderType = $routeParams.folderType;// ? $routeParams.folderType : 'Masters';
            var themeId = $routeParams.themeId;
            $scope.listUrl = '/portal/template/list/' + themeId + '?folderType=' + encodeURIComponent($scope.folderType);
            var resp = await service.getSingle(['portal', themeId, $scope.folderType, id]);
            if (resp && resp.isSucceed) {
                $scope.activedData = resp.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        $scope.getList = async function (pageIndex, themeId) {
            $scope.themeId = themeId || $routeParams.themeId;
            $scope.request.key = $routeParams.folderType;
            $scope.folderType = $routeParams.folderType;
            if ($scope.folderType) {
                if (pageIndex !== undefined) {
                    $scope.request.pageIndex = pageIndex;
                }
                if ($scope.request.fromDate !== null) {
                    var d = new Date($scope.request.fromDate);
                    $scope.request.fromDate = d.toISOString();
                }
                if ($scope.request.toDate !== null) {
                    var d = new Date($scope.request.toDate);
                    $scope.request.toDate = d.toISOString();
                }
                var resp = await service.getList($scope.request, [$scope.themeId]);
                if (resp && resp.isSucceed) {
                    $scope.data = resp.data;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            } else {
                $rootScope.isBusy = false;
            }
        };
    }]);
