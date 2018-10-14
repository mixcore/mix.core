'use strict';
app.controller('LanguageController',
    ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location', 'LanguageService', 'CommonService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location, service, commonService) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.languageFile = {
                file: null,
                fullPath: '',
                folder: 'Language',
                title: '',
                description: ''
            };
            $scope.cates = [
                {
                    title: 'Common',
                    prefix: ''
                },
                {
                    title: 'Portal',
                    prefix: 'portal_'
                },
                {
                    title: 'Frontend',
                    prefix: 'fe_'
                }
            ];
            $scope.cate = $scope.cates[0];
            $scope.dataTypes = ngAppSettings.editorConfigurations.dataTypes;
            
            $scope.saveCallback = function () {
                commonService.initAllSettings().then(function () {
                    $location.path('/portal/language/list');
                });
            }
            $scope.removeCallback = function () {
                commonService.initAllSettings().then(function () {
                    $location.path('/portal/language/list');
                });
            }
            $scope.generateDefault = function (text, cate) {
                if (!$routeParams.id) {
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
