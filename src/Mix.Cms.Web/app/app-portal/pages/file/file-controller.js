'use strict';
app.controller('FileController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$timeout', '$location', 'AuthService', 'FileServices',
    function ($scope, $rootScope, ngAppSettings, $routeParams, $timeout, $location, authService, fileServices) {
        $scope.request = {
            pageSize: '10',
            pageIndex: 0,
            status: '2',
            orderBy: 'CreatedDateTime',
            direction: '1',
            fromDate: null,
            toDate: null,
            keyword: '',
            key: ''
        };

        $scope.activedFile = null;
        $scope.relatedFiles = [];
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

        $scope.loadFile = async function () {
            $rootScope.isBusy = true;
            $scope.listUrl = '/portal/file/list?folder=' + $routeParams.folder;
            $rootScope.isBusy = true;
            var response = await fileServices.getFile($routeParams.folder, $routeParams.filename);
            if (response.isSucceed) {
                $scope.activedFile = response.data;                
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        $scope.loadFiles = async function (folder) {
            if (folder) {
                $scope.request.key += ($scope.request.key !== '') ? '/' : '';
                $scope.request.key += folder;
            } else {
                $scope.request.key = $routeParams.folder ? $routeParams.folder : '';
            }
            $rootScope.isBusy = true;
            var resp = await fileServices.getFiles($scope.request);
            if (resp && resp.isSucceed) {

                ($scope.data = resp.data);
                $.each($scope.data.items, function (i, file) {

                    $.each($scope.activedFiles, function (i, e) {
                        if (e.fileId === file.id) {
                            file.isHidden = true;
                        }
                    })
                })
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.removeFile = async function (id) {
            if (confirm("Are you sure!")) {
                $rootScope.isBusy = true;
                var resp = await fileServices.removeFile(id);
                if (resp && resp.isSucceed) {
                    $scope.loadFiles();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            }
        };

        $scope.saveFile = async function (file) {            
            $rootScope.isBusy = true;
            var resp = await fileServices.saveFile(file);
            if (resp && resp.isSucceed) {
                $scope.activedFile = resp.data;
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
