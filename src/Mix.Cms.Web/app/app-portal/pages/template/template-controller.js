'use strict';
app.controller('TemplateController', ['$scope', '$rootScope', '$routeParams', 'ngAppSettings', 'TemplateService',
    function ($scope, $rootScope, $routeParams, ngAppSettings, service) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
        $scope.folderTypes = [
            'Masters',
            'Layouts',
            'Pages',
            'Modules',
            'Products',
            'Articles',
            'Widgets',
        ];
        $scope.loadParams = async function () {
            $rootScope.isBusy = true;
            $scope.folderType = $routeParams.folderType ? $routeParams.folderType : 'Masters';
            $scope.backUrl = '/portal/template/list/' + $routeParams.themeId;
            $scope.themeId = $routeParams.themeId;
        }
        $scope.getSingle = async function () {
            $rootScope.isBusy = true;
            var id = $routeParams.id;
            var folderType = $routeParams.folderType ? $routeParams.folderType : 'Masters';
            var themeId = $routeParams.themeId;
            var resp = await service.getSingle(['portal', themeId, folderType, id]);
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
            $scope.request.key = this.folderType;
            $scope.folderType = this.folderType;
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
        };
    }]);
