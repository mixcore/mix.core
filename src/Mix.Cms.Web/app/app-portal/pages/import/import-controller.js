'use strict';
app.controller('ImportFileController',
    ['$scope', '$rootScope', 'ImportFileServices', 'TranslatorService', 'GlobalSettingsService',
        function ($scope, $rootScope, service, translatorService, GlobalSettingsService) {

            $scope.saveImportFile = async function () {
                $rootScope.isBusy = true;
                var form = document.getElementById('frm-import');
                var frm = new FormData();
                frm.append( 'assets',  form['assets'].files[0]);
                var response = await service.saveImportFile(frm);
                if (response.isSucceed) {
                    $scope.activedData = response.data;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    $rootScope.showErrors(response.errors);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
        }]);
