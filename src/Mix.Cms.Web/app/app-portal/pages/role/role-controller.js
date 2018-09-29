'use strict';
app.controller('RoleController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$timeout', '$location', 'AuthService', 'RoleServices',
    function ($scope, $rootScope, ngAppSettings, $routeParams, $timeout, $location, authService, roleServices) {
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
        $scope.activedRole = null;
        $scope.relatedRoles = [];
        $rootScope.isBusy = false;
        $scope.data = {
            pageIndex: 0,
            pageSize: 1,
            totalItems: 0
        };
        $scope.errors = [];
        $scope.role = { name: '' };
        $scope.range = function (max) {
            var input = [];
            for (var i = 1; i <= max; i += 1) input.push(i);
            return input;
        };

        $scope.loadRole = async function () {
            $rootScope.isBusy = true;
            var id = $routeParams.id;
            var response = await roleServices.getRole(id, 'portal');
            if (response.isSucceed) {
                $scope.activedRole = response.data;
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

        $scope.loadRoles = async function (pageIndex) {
            if (pageIndex !== undefined) {
                $scope.request.pageIndex = pageIndex;
            }
            $rootScope.isBusy = true;
            var resp = await roleServices.getRoles($scope.request);
            if (resp && resp.isSucceed) {
                $scope.data = resp.data;
                $.each($scope.data.items, function (i, role) {

                    $.each($scope.data, function (i, e) {
                        if (e.roleId === role.id) {
                            role.isHidden = true;
                        }
                    });
                });
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.removeRole = function (id) {
            $rootScope.showConfirm($scope, 'removeRoleConfirmed', [id], null, 'Remove Role', 'Are you sure');
        };

        $scope.removeRoleConfirmed = async function (id) {
            $rootScope.isBusy = true;
            var result = await roleServices.removeRole(id);
            if (result.isSucceed) {
                $scope.loadRoles();
            }
            else {
                $rootScope.showMessage('failed');
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.createRole = async function () {
            $rootScope.isBusy = true;
            var result = await roleServices.createRole($scope.role.name);
            if (result.isSucceed) {
                $scope.role.name = '';
                $scope.loadRoles();
            }
            else {
                $rootScope.showMessage(result.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };


        $scope.saveRole = async function (role) {
            $rootScope.isBusy = true;
            var resp = await roleServices.saveRole(role);
            if (resp && resp.isSucceed) {
                $scope.activedRole = resp.data;
                $rootScope.showMessage('Thành công', 'success');
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
