'use strict';
app.controller('LanguageController',
    ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location', 'LanguageService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location, service, commonService) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.cates = ngAppSettings.enums.language_types;
            $scope.settings = $rootScope.globalSettings;
            $scope.languageFile = {
                file: null,
                fullPath: '',
                folder: 'Language',
                title: '',
                description: ''
            };                
            $scope.cate = $scope.cates[0];
            $scope.dataTypes = $rootScope.globalSettings.dataTypes;
            
            $scope.saveSuccessCallback = function () {
                commonService.initAllSettings().then(function () {
                    $location.url($scope.referrerUrl);
                });
            }
            $scope.removeCallback = function () {
                commonService.initAllSettings().then(function () {
                    $location.url($scope.referrerUrl);
                });
            }
            $scope.generateDefault = function (text, cate) {
                if (!$routeParams.id && !$scope.activedData.keyword) {
                    $scope.activedData.defaultValue = text;
                    $scope.activedData.keyword = cate.prefix + text.replace(/[^a-zA-Z0-9]+/g, '_')
                        .replace(/([A-Z]+)([A-Z][a-z])/g, '$1-$2')
                        .replace(/([a-z])([A-Z])/g, '$1-$2')
                        .replace(/([0-9])([^0-9])/g, '$1-$2')
                        .replace(/([^0-9])([0-9])/g, '$1-$2')
                        .replace(/-+/g, '_')
                        .toLowerCase();
                }
            };
    
        }]);
