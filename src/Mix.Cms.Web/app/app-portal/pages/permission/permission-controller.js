'use strict';
app.controller('PermissionController',
    ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location', 'PermissionService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location, service, commonService) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.request.query = "level=0";
            $scope.miOptions=ngAppSettings.miIcons;
            $scope.columns = [
                {
                    title: 'Keyword',
                    name: 'textKeyword',
                    filter: true,
                    type: 0 // string - ngAppSettings.dataTypes[0]
                },
                {
                    title: 'Default',
                    name: 'textDefault',
                    filter: true,
                    type: 0// string - ngAppSettings.dataTypes[0]
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
            $scope.initCurrentPath = async function(){
                
                var resp = await service.getSingle([null, 'portal']);
                if (resp && resp.isSucceed) { 
                    $scope.activedData = resp.data;
                    $scope.activedData.url = $location.path();
                    $rootScope.isBusy = false;
                    $scope.$applyAsync();
                }
                else { 
                    if (resp) {
                        $rootScope.showErrors(resp.errors);
                    }
                    if ($scope.getSingleFailCallback) {
                        $scope.getSingleFailCallback();
                    }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                
            };
            $scope.saveCallback = function(){
                $scope.getSingle();
            }
            
            $scope.updateInfos = async function (items) {
                $rootScope.isBusy = true;
                var resp = await service.updateInfos(items);
                if (resp && resp.isSucceed) {
                    $scope.activedPage = resp.data;
                    $rootScope.showMessage('success', 'success');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
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
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            $('#dlg-favorite').on('show.bs.modal', function (event) {
                $scope.initCurrentPath();
              });
        }]);
