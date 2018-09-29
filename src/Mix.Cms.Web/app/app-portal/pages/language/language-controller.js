'use strict';
app.controller('LanguageController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$timeout', '$location'
    , 'AuthService', 'LanguageServices', 'CommonService', 'TranslatorService',
    function ($scope, $rootScope, ngAppSettings, $routeParams, $timeout, $location, authService, languageServices, commonServices, translatorService) {
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
        $scope.activedLanguage = null;
        $scope.relatedLanguages = [];
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
        $scope.initEditors = function () {
            $rootScope.initEditor();
        }
        $scope.getLanguage = async function (id) {
            $rootScope.isBusy = true;
            var resp = await languageServices.getLanguage(id, 'portal');
            if (resp && resp.isSucceed) {

                $scope.activedLanguage = resp.data;
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
        $scope.loadLanguage = async function () {
            $rootScope.isBusy = true;
            var id = $routeParams.id;
            var response = await languageServices.getLanguage(id, 'api');
            if (response.isSucceed) {
                $scope.activedLanguage = response.data;
                if (!id) {
                    $scope.activedLanguage.category = 'Common';
                }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.loadLanguages = async function (pageIndex) {
            if (pageIndex !== undefined) {
                $scope.request.pageIndex = pageIndex;
            }
            $rootScope.isBusy = true;
            var resp = await languageServices.getLanguages($scope.request);
            if (resp && resp.isSucceed) {

                $scope.data = resp.data;
                $.each($scope.data.items, function (i, language) {

                    $.each($scope.activedLanguages, function (i, e) {
                        if (e.languageId === language.id) {
                            language.isHidden = true;
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

        $scope.removeLanguage = function (id) {
            $rootScope.showConfirm($scope, 'removeLanguageConfirmed', [id], null, 'Remove Language', 'Are you sure');
        };

        $scope.removeLanguageConfirmed = async function (id) {
            $rootScope.isBusy = true;
            var result = await languageServices.removeLanguage(id);
            if (result.isSucceed) {
                commonServices.removeTranslator();
                $scope.loadLanguages();
            }
            else {
                $rootScope.showMessage('failed');
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.saveLanguage = async function (language) {
            language.content = $('.editor-content').val();
            language.dataType = language.property.dataType;
            var resp = await languageServices.saveLanguage(language);
            if (resp && resp.isSucceed) {
                $scope.activedLanguage = resp.data;
                commonServices.removeTranslator();
                $rootScope.showMessage('success', 'success');

                translatorService.reset($rootScope.globalSettings.lang).then(function () {
                    window.location.href = '/portal/language/list';
                });

            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.generateDefault = function (text, cate) {
            if (!$routeParams.id) {
                $scope.activedLanguage.defaultValue = text;
                $scope.activedLanguage.keyword = cate.prefix + text.replace(/[^a-zA-Z0-9]+/g, '_')
                    .replace(/([A-Z]+)([A-Z][a-z])/g, '$1-$2')
                    .replace(/([a-z])([A-Z])/g, '$1-$2')
                    .replace(/([0-9])([^0-9])/g, '$1-$2')
                    .replace(/([^0-9])([0-9])/g, '$1-$2')
                    .replace(/-+/g, '_')
                    .toLowerCase();
            }
        };

    }]);
