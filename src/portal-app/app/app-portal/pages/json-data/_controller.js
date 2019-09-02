'use strict';
app.controller('JsonDataController', 
    ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$timeout', '$location', 'AuthService', 'JsonDataService',
    function ($scope, $rootScope, ngAppSettings, $routeParams, $timeout, $location, authService, service) {
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
        $scope.editorVisible = false;
        $scope.data = null;
        $scope.relatedFiles = [];
        $rootScope.isBusy = false;

        $scope.errors = [];

        $scope.loadPage = async function(folder){
            if (folder) {
                $scope.request.key += ($scope.request.key !== '') ? '/' : '';
                $scope.request.key += folder;
            }
            $location.url('/portal/json-data/list?folder=' + encodeURIComponent($scope.request.key));
        };
        $scope.init = async function () {
            $rootScope.isBusy = true;
            $scope.filename = $routeParams.filename;
            $scope.folder = $routeParams.folder;
            $scope.listUrl = '/portal/json-data/list?folder=' + $routeParams.folder;
            $rootScope.isBusy = true;
            var response = await service.getFile($routeParams.folder, $routeParams.filename);
            if (response.isSucceed) {
                $scope.activedFile = response.data;
                $scope.data = $.parseJSON(response.data.content); 
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
                $scope.request.key = $routeParams.folder ? $routeParams.folder : 'data';
            }
            
            $rootScope.isBusy = true;
            var resp = await service.getFiles($scope.request);
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
                var resp = await service.removeFile(id);
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
        $scope.selectPane = function(pane){
            $scope.editorVisible = pane.header == 'Content';
        };
        $scope.save = async function(data){
            $scope.activedFile.contennt = JSON.stringify(data);
            $scope.saveFile($scope.activedFile);
        }
        $scope.updateData = async function(data){
            $scope.data = data;
        }
        $scope.saveFile = async function (file) {            
            $rootScope.isBusy = true;
            file.content = JSON.stringify($scope.data);
            var resp = await service.saveFile(file);
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
