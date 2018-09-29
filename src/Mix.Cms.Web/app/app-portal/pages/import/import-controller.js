'use strict';
app.controller('ImportFileController',
    ['$scope', '$rootScope', 'ImportFileServices', 'TranslatorService', 'GlobalSettingsService', 
        function ($scope, $rootScope, importFileServices, translatorService, GlobalSettingsService) {

            $scope.importFile = {
                title: '',
                description: '',
                postedFile: {
                    file: null,
                    fullPath: '',
                    folderName: 'Import',
                    fileFolder: 'Import',
                    fileName: '',
                    extension: '',
                    content: '',
                    fileStream: ''
                }
            };
            $scope.importType = 'Language';
            $scope.types = [
                'Language',
                'Configuration'
            ];
            $scope.errors = [];
            $scope.saveImportFile = async function (importFile, importType) {
                $rootScope.isBusy = true;
                var resp = await importFileServices.saveImportFile(importFile.postedFile, importType);
                if (resp && resp.isSucceed) {
                    $scope.activedImportFile = resp.data;
                    $rootScope.showMessage('Success', 'success');
                    switch (importType) {
                        case 'Language':
                            translatorService.reset($rootScope.globalSettings.lang).then(function () {
                                window.location.href = '/portal/language/list';
                            });
                            return false;
                        case 'Configuration':
                            GlobalSettingsService.reset($rootScope.globalSettings.lang).then(function () {
                                window.location.href = '/portal/configuration/list';
                            });
                            return false;
                    }


                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
        }]);
