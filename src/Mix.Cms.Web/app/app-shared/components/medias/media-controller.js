'use strict';
app.controller('MediaController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$timeout', '$location', 'AuthService', 'MediaService',
    function ($scope, $rootScope, ngAppSettings, $routeParams, $timeout, $location, authService, mediaService) {
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
        $scope.activedMedia = {
            title: '',
            description: '',
            mediaFile: {
                file: null,
                fullPath: '',
                folderName: 'Media',
                fileFolder: '',
                fileName: '',
                extension: '',
                content: '',
                fileStream: ''
            }
        };
        $scope.relatedMedias = [];
        $rootScope.isBusy = false;
        $scope.data = {
            pageIndex: 0,
            pageSize: 1,
            totalItems: 0
        };
        $scope.errors = [];

        $scope.range = function (max) {
            var input = [];
            for (var i = 1; i <= max; i += 1) input.push(i);
            return input;
        };

        $scope.getMedia = async function (id) {
            $rootScope.isBusy = true;
            var resp = await mediaService.getMedia(id, 'portal');
            if (resp && resp.isSucceed) {
                $scope.activedMedia = resp.data;
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

        $scope.uploadMedia = async function () {
            $rootScope.isBusy = true;
            var resp = await mediaService.uploadMedia($scope.mediaFile);
            if (resp && resp.isSucceed) {
                $scope.activedMedia = resp.data;
                $scope.loadMedias();
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.loadMedia = async function () {
            $rootScope.isBusy = true;
            var id = $routeParams.id;
            var response = await mediaService.getMedia(id, 'portal');
            if (response.isSucceed) {
                $scope.activedMedia = response.data;
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
        $scope.loadMedias = async function (pageIndex) {
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
            if ($rootScope.globalSettings) {
                $rootScope.isBusy = true;
                var resp = await mediaService.getList($scope.request);
                if (resp && resp.isSucceed) {
                    $rootScope.medias = resp.data;
                    $scope.data = resp.data;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            }
        };

        $scope.removeMedia = async function (id) {
            if (confirm("Are you sure!")) {
                $rootScope.isBusy = true;
                var resp = await mediaService.removeMedia(id);
                if (resp && resp.isSucceed) {
                    $scope.loadMedias();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            }
        };

        $scope.selectFile = function (file, errFiles) {
            if (file !== undefined && file !== null) {
                $scope.mediaFile.folder = 'Media';
                $scope.mediaFile.file = file;
                $scope.getBase64(file);
            }
        };

        $scope.getBase64 = function (file) {
            if (file !== null && $scope.postedFile) {
                $rootScope.isBusy = true;
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = function () {
                    var index = reader.result.indexOf(',') + 1;
                    var base64 = reader.result.substring(index);
                    $scope.activedMedia.mediaFile.fileName = $rootScope.generateKeyword(file.name.substring(0, file.name.lastIndexOf('.')), '-');
                    $scope.activedMedia.mediaFile.extension = file.name.substring(file.name.lastIndexOf('.'));
                    $scope.activedMedia.mediaFile.fileStream = reader.result;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                };
                reader.onerror = function (error) {
                    $rootScope.showErrors([error]);
                    $rootScope.isBusy = false;
                };
            }
            else {
                return null;
            }
        };

        $scope.saveMedia = async function (media) {
            $rootScope.isBusy = true;
            var resp = await mediaService.saveMedia(media);
            if (resp && resp.isSucceed) {
                $scope.activedMedia = resp.data;
                $rootScope.showMessage('Thành công', 'success');
                $scope.loadMedias();
                $scope.loadMedia();
                $scope.$apply();
                //$location.path('/portal/media/details/' + resp.data.id);
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        $scope.togglePreview = function (item) {
            item.isPreview = item.isPreview === undefined ? true : !item.isPreview;
        };
        $scope.clone = async function (id) {
            $rootScope.isBusy = true;
            var resp = await mediaService.cloneMedia(id);
            if (resp && resp.isSucceed) {
                $scope.activedMedia = resp.data;
                $rootScope.showMessage('Thành công', 'success');
                $rootScope.isBusy = false;
                $scope.$apply();
                //$location.path('/portal/media/details/' + resp.data.id);
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        var json = {
            src: 'fonts/font-awsome.ttf',
            title: 'Font Awsome',
            data: [
                {
                    title: 'Facebook',
                    value: 'fa-facebook'
                },
                {
                    title: 'Twitter',
                    value: 'fa-twitter'
                }
            ]
        };
    }]);
