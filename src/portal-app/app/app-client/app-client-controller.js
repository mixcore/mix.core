(function (angular) {
    'use strict';
    app.controller('AppClientController',
        ['$rootScope', '$scope', 'GlobalSettingsService', 'CommonService', 'AuthService', 'BaseService'
            , 'localStorageService', 'TranslatorService', 'SharedModuleDataService',
            function ($rootScope, $scope, globalSettingsService, commonService, authService, baseService,
                localStorageService, translatorService, moduleDataService) {
                $scope.lang = '';
                $scope.isInit = false;
                $scope.mediaFile = {
                    file: null,
                    fullPath: '',
                    folder: 'module-data',
                    title: '',
                    description: ''
                };
                $scope.cartData = {
                    items: [],
                    totalItems: 0,
                    total: 0,
                };
                $rootScope.globalSettingsService = globalSettingsService;
                $scope.changeLang = $rootScope.changeLang;
                $scope.init = function (lang) {
                    if (!$rootScope.isBusy) {
                        $rootScope.isBusy = true;
                        // globalSettingsService.fillGlobalSettings().then(function (response) {
                        $scope.cartData = localStorageService.get('shoppingCart');
                        if (!$scope.cartData) {
                            $scope.cartData = {
                                items: [],
                                totalItems: 0,
                                total: 0,
                            };
                        }
                        commonService.fillAllSettings(lang).then(function (response) {
                            if ($rootScope.globalSettings) {
                                authService.fillAuthData().then(function (response) {
                                    $rootScope.authentication = authService.authentication;
                                    $scope.isInit = true;
                                    $rootScope.isInit = true;
                                    $rootScope.isBusy = false;
                                    $scope.$apply();
                                });

                                // });                                
                            } else {
                                $scope.isInit = true;
                                $rootScope.isInit = true;
                                $rootScope.isBusy = false;
                            }
                        });

                        // });

                    }
                };

                $scope.translate = $rootScope.translate;
                $scope.previewData = function (moduleId, id) {
                    var obj = {
                        moduleId: moduleId,
                        id: id
                    };
                    $rootScope.preview('module-data', obj, null, 'modal-lg');
                }

                $scope.initModuleForm = async function (name, successCallback, failCallback) {
                    var resp = null;
                    $scope.successCallback = successCallback;
                    $scope.failCallback = failCallback;
                    setTimeout(async () => {
                        $scope.name = name;
                        if ($scope.id) {
                            resp = await moduleDataService.getModuleData($scope.id, $scope.dataId, 'portal');
                        }
                        else {
                            resp = await moduleDataService.initModuleForm($scope.name);
                        }

                        if (resp && resp.isSucceed) {
                            $scope.activedModuleData = resp.data;
                            $rootScope.isBusy = false;
                            $scope.$apply();
                        }
                        else {
                            if (resp) {
                                if ($scope.errorCallback) {
                                    $rootScope.executeFunctionByName($scope.errorCallback, [resp], window);
                                }
                                else {
                                    $rootScope.showErrors(resp.errors);
                                }
                            }
                            $rootScope.isBusy = false;
                            $scope.$apply();
                        }
                    }, 500);
                };
                $scope.saveEmployee = function () {
                    var msg = $rootScope.settings.data['employee_confirm_msg'] || 'Are you sure you want to submit this form? Please be noted that after submission, all information cannot be changed or adjusted.';
                    $rootScope.showConfirm($scope, 'saveEmployeeConfirmed', [], null, '', msg);
                };
                $scope.saveEmployeeConfirmed = async function () {
                    if ($scope.validateEmployee($scope.activedModuleData.jItem)) {
                        var mediaService = angular.copy(baseService);
                        mediaService.init('media');
                        $rootScope.isBusy = true;
                        var getMedia = await mediaService.getSingle(['portal']);
                        if (getMedia.isSucceed) {
                            // mediaFile.fileName = file.name.substring(0, file.name.lastIndexOf('.'));
                            // mediaFile.extension = file.name.substring(file.name.lastIndexOf('.'));
                            // mediaFile.fileStream = imgData;
                            var media = getMedia.data;
                            media.title = '';
                            media.description = '';
                            media.mediaFile = $scope.mediaFile;
                            var resp = await mediaService.save(media);
                            if (resp && resp.isSucceed) {
                                $scope.activedModuleData.jItem['avatar'].value = resp.data.fullPath;
                                return $scope.saveModuleData();
                            }
                            else {
                                if (resp) { $rootScope.showErrors(resp.errors); }
                                $rootScope.isBusy = false;
                                $scope.$apply();
                            }
                        }

                    }
                };
                $scope.validateEmployee = function (data) {
                    var year = parseInt(data.start_year.value);
                    if (year < 1900 || year > 2019) {
                        $rootScope.showErrors([$rootScope.translate('invalid_year')]);
                        return false;
                    } else if (!$scope.mediaFile.fileName) {
                        $rootScope.showErrors([$rootScope.translate('avatar_required')]);
                        return false;
                    } else {
                        return true;
                    }

                };
                $scope.saveModuleData = async function () {

                    var resp = await moduleDataService.saveModuleData($scope.activedModuleData);
                    if (resp && resp.isSucceed) {
                        $scope.activedModuleData = resp.data;
                        if ($scope.successCallback) {
                            $rootScope.executeFunctionByName($scope.successCallback, [resp], window);
                        }
                        else {
                            var msg = $rootScope.settings.data['employee_success_msg'] || 'Thank you for submitting! Your lovely photo is well received 😊';
                            $rootScope.showConfirm($scope, '', [], null, '', msg);
                        }

                        $rootScope.isBusy = false;
                        $scope.initModuleForm($scope.name);
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                    else {
                        if (resp) {
                            if ($scope.failCallback) {
                                $rootScope.executeFunctionByName($scope.failCallback, [resp], window);
                            }
                            else {
                                $rootScope.showErrors(resp.errors);
                            }
                        }
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                };
                $scope.shareFB = function (url) {
                    FB.ui({
                        method: 'share',
                        href: url,
                    }, function (response) { });
                }
                $scope.shareTwitter = function (url, content) {
                    var text = encodeURIComponent(content);
                    var shareUrl = 'https://twitter.com/intent/tweet?url=' + url + '&text=' + text;
                    var win = window.open(shareUrl, 'ShareOnTwitter', getWindowOptions());
                    win.opener = null; // 2
                }
                $scope.saveShoppingCart = function () {
                    localStorageService.set('shoppingCart', $scope.cartData);
                }

                var getWindowOptions = function () {
                    var width = 500;
                    var height = 350;
                    var left = (window.innerWidth / 2) - (width / 2);
                    var top = (window.innerHeight / 2) - (height / 2);

                    return [
                        'resizable,scrollbars,status',
                        'height=' + height,
                        'width=' + width,
                        'left=' + left,
                        'top=' + top,
                    ].join();
                };
            }]);


})(window.angular);