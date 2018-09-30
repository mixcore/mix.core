'use strict';
app.controller('ModuleDataController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$timeout', '$location', 'AuthService', 'ModuleDataService',
    function ($scope, $rootScope, ngAppSettings, $routeParams, $timeout, $location, authService, moduleDataService) {
        $scope.request = {
            pageSize: '10',
            pageIndex: 0,
            status: '2',
            orderBy: 'CreatedDateTime',
            direction: '1',
            fromDate: null,
            toDate: null,
            keyword: ''
        };
        $scope.moduleDataFile = {
            file: null,
            fullPath: '',
            folder: 'ModuleData',
            title: '',
            description: ''
        };
        $scope.activedModuleData = null;
        $scope.relatedModuleDatas = [];
        $rootScope.isBusy = false;
        $scope.data = {
            pageIndex: 0,
            pageSize: 1,
            totalItems: 0,
        };
        $scope.errors = [];

        $scope.range = function (max) {
            var input = [];
            for (var i = 1; i <= max; i += 1) input.push(i);
            return input;
        };

        $scope.getModuleData = async function (id) {
            $rootScope.isBusy = true;
            var resp = await moduleDataService.getModuleData(id, 'portal');
            if (resp && resp.isSucceed) {
                $scope.activedModuleData = resp.data;
                $rootScope.initEditor();
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.initModuleForm = async function (name) {
            var resp = null;
            $scope.name = name;
            if ($scope.id) {
                resp = await moduleDataService.getModuleData($scope.id, $scope.dataId, 'portal');
            }
            else {
                resp = await moduleDataService.initModuleForm($scope.name);
            }

            if (resp && resp.isSucceed) {
                $scope.activedModuleData = resp.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.loadParams = async function () {
            $scope.dataId = $routeParams.id;
            $scope.backUrl = '/portal/module/data/' + $routeParams.moduleId;
            $scope.moduleId = $routeParams.moduleId;
        };

        $scope.loadModuleData = async function () {
            $rootScope.isBusy = true;
            var moduleId = $routeParams.moduleId;
            var id = $routeParams.id;
            var response = await moduleDataService.getModuleData(moduleId, id, 'portal');
            if (response.isSucceed) {
                $scope.activedModuleData = response.data;
                $rootScope.initEditor();
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        $scope.loadModuleDatas = async function (moduleId, pageIndex) {
            $scope.request.key = moduleId;
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
            var resp = await moduleDataService.getModuleDatas($scope.request);
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

        $scope.removeModuleData = async function (id) {
            if (confirm("Are you sure!")) {
                var resp = await moduleDataService.removeModuleData(id);
                if (resp && resp.isSucceed) {
                    $scope.loadModuleDatas();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                }
            }
        };

        $scope.saveModuleData = async function () {

            var resp = await moduleDataService.saveModuleData($scope.activedModuleData);
            if (resp && resp.isSucceed) {
                $scope.activedModuleData = resp.data;
                $rootScope.showMessage('Thành công', 'success');
                $rootScope.isBusy = false;
                $scope.initModuleForm($scope.name);
                $rootScope.isBusy = false;
                $scope.$apply();
                //$location.path('/portal/moduleData/details/' + resp.data.id);
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

    }]);
