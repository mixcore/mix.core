'use strict';
app.controller('ModalArticleController', [
    '$scope', '$rootScope', '$location', 'ngAppSettings', '$routeParams', 'ArticleService',
    function (
        $scope, $rootScope, $location, ngAppSettings, $routeParams, service) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);

        $scope.activedData = {
            title: '',
            description: '',
            status: 2,
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
        $scope.uploadMedia = async function () {
            $rootScope.isBusy = true;
            var resp = await service.uploadMedia($scope.mediaFile);
            if (resp && resp.isSucceed) {
                $scope.activedMedia = resp.data;
                $scope.getList();
                $scope.$apply();
            } else {
                if (resp) {
                    $rootScope.showErrors(resp.errors);
                }
                $rootScope.isBusy = false;
                $scope.$apply();
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
            } else {
                return null;
            }
        };
        $scope.togglePreview = function (item) {
            item.isArPreview = item.isArPreview === undefined ? true : !item.isArPreview;
        };
        $scope.clone = async function (id) {
            $rootScope.isBusy = true;
            var resp = await service.cloneMedia(id);
            if (resp && resp.isSucceed) {
                $scope.activedMedia = resp.data;
                $rootScope.showMessage('Thành công', 'success');
                $rootScope.isBusy = false;
                $scope.$apply();
                //$location.path('/portal/media/details/' + resp.data.id);
            } else {
                if (resp) {
                    $rootScope.showErrors(resp.errors);
                }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        $scope.saveCallback = function () {
            $scope.getList();
        }
        $scope.removeCallback = function () {
            $scope.getList();
        }

        $scope.request.query = "level=0";
        $scope.miOptions = ngAppSettings.miIcons;
        $scope.columns = [{
                title: 'Keyword',
                name: 'textKeyword',
                filter: true,
                type: 0 // string - ngAppSettings.dataTypes[0]
            },
            {
                title: 'Default',
                name: 'textDefault',
                filter: true,
                type: 0 // string - ngAppSettings.dataTypes[0]
            },
            {
                title: 'Url',
                name: 'url',
                filter: true,
                type: 0 // string - ngAppSettings.dataTypes[0]
            },
            {
                title: 'Created Date',
                name: 'createdDateTime',
                filter: true,
                type: 0 // string - ngAppSettings.dataTypes[0]
            },
        ];
        $scope.initCurrentPath = async function () {
            await $scope.getSingle();
            $scope.activedData.url = $location.path();
            $scope.$applyAsync();
        };

        $scope.updateInfos = async function (items) {
            $rootScope.isBusy = true;
            var resp = await service.updateInfos(items);
            if (resp && resp.isSucceed) {
                $scope.activedPage = resp.data;
                $rootScope.showMessage('success', 'success');
                $rootScope.isBusy = false;
                $scope.$apply();
            } else {
                if (resp) {
                    $rootScope.showErrors(resp.errors);
                }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.updateChildInfos = async function (items) {
            $rootScope.isBusy = true;
            var resp = await service.updateChildInfos(items);
            if (resp && resp.isSucceed) {
                $scope.activedPage = resp.data;
                $rootScope.showMessage('success', 'success');
                $rootScope.isBusy = false;
                $scope.$apply();
            } else {
                if (resp) {
                    $rootScope.showErrors(resp.errors);
                }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        $('#dlg-favorite').on('show.bs.modal', function (event) {
            $scope.initCurrentPath();
        });
    }
]);