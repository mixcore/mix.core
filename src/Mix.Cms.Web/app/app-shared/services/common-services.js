'use strict';
app.factory('CommonService', ['$location', '$http', '$rootScope', 'AuthService', 'localStorageService', 'AppSettings',
    function ($location, $http, $rootScope, authService, localStorageService, appSettings) {
        var adminCommonFactory = {};
        var _settings = {
            lang: '',
            cultures: []
        };
        var _showAlertMsg = function (title, message) {
            $rootScope.message = {
                title: title,
                value: message
            };
            $('#modal-msg').modal('show');
        };

        var _checkfile = function (sender, validExts) {
            var fileExt = sender.value;
            fileExt = fileExt.substring(fileExt.lastIndexOf('.'));
            if (validExts.indexOf(fileExt) < 0) {
                _showAlertMsg("", "Invalid file selected, valid files are of " + validExts.toString() + " types.");
                sender.value = "";
                return false;
            }
            else return true;
        };

        var _getSettings = async function (culture) {
            var settings = localStorageService.get('settings');
            // && culture !== undefined && settings.lang === culture
            if (settings) {
                return settings;
            }
            else {
                var url = '/portal';
                if (culture) {
                    url += '/' + culture;
                }
                url += '/all-settings';
                var req = {
                    method: 'GET',
                    url: url
                };
                return _getApiResult(req).then(function (response) {
                    return response.data;
                });
            }
        };

        var _setSettings = async function (settings) {
            if (settings && settings.cultures.length > 0) {
                localStorageService.set('settings', settings);
            }
        };

        var _initAllSettings = async function (culture) {
            localStorageService.remove('settings');
            localStorageService.remove('translator');
            localStorageService.remove('globalSettings');

            var response = await _getSettings(culture);
            localStorageService.set('settings', response.settings);
            localStorageService.set('translator', response.translator);
            localStorageService.set('globalSettings', response.globalSettings);

            return response;
        };

        var _removeSettings = async function (settings) {
            localStorageService.remove('settings');
        };

        var _removeTranslator = async function () {
            localStorageService.remove('translator');
        };

        var _fillSettings = async function (culture) {
            var settings = localStorageService.get('settings');
            if (settings && settings.lang === culture) {
                _settings = settings;
                return settings;
            }
            else {
                if (culture !== undefined && settings && settings.lang !== culture) {
                    await _removeSettings();
                    await _removeTranslator();
                }
                settings = await _getSettings(culture);
                localStorageService.set('settings', settings);
                //window.top.location = location.href;
                return settings;
            }

        };
        var _getApiResult = async function (req, serviceBase) {
            $rootScope.isBusy = true;
            if (!authService.authentication) {
                await authService.fillAuthData();
            }
            if (authService.authentication) {
                req.Authorization = authService.authentication.token;
            }

            var serviceUrl = appSettings.serviceBase + '/api/' + appSettings.apiVersion;
            if (serviceBase !== undefined) {
                serviceUrl = serviceBase + '/api/' + appSettings.apiVersion
            }

            req.url = serviceUrl + req.url;
            if (!req.headers) {
                req.headers = {
                    'Content-Type': 'application/json'
                };
            }
            req.headers.Authorization = 'Bearer ' + req.Authorization || '';
            return $http(req).then(function (resp) {
                //var resp = results.data;

                return resp.data;
            },
                function (error) {
                    if (error.status === 401) {
                        //Try again with new token from previous Request (optional)                
                        return authService.refreshToken(authService.authentication.refresh_token).then(function () {
                            req.headers.Authorization = 'Bearer ' + authService.authentication.token;
                            return $http(req).then(function (results) {

                                return results.data;
                            }, function (err) {

                                authService.logOut();
                                authService.authentication.token = null;
                                authService.authentication.refresh_token = null;
                                authService.referredUrl = $location.$$url;
                                window.top.location.href = '/init/login';
                            });
                        }, function (err) {

                            var t = { isSucceed: false, errors: [err.status] };

                            authService.logOut();
                            authService.authentication.token = null;
                            authService.authentication.refresh_token = null;
                            authService.referredUrl = $location.$$url;
                            window.top.location.href = '/init/login';
                            return t;
                        }
                        );
                    }
                    else if (error.status === 403) {
                        var t = { isSucceed: false, errors: ['Forbidden'] };
                        window.top.location.href = '/init/login';
                        return t;
                    }
                    else {
                        return { isSucceed: false, errors: [error.statusText || error.status] };
                    }
                });
        };
        adminCommonFactory.getApiResult = _getApiResult;
        adminCommonFactory.getSettings = _getSettings;
        adminCommonFactory.setSettings = _setSettings;
        adminCommonFactory.initAllSettings = _initAllSettings;
        adminCommonFactory.removeSettings = _removeSettings;
        adminCommonFactory.removeTranslator = _removeTranslator;
        adminCommonFactory.showAlertMsg = _showAlertMsg;
        adminCommonFactory.checkfile = _checkfile;
        adminCommonFactory.fillSettings = _fillSettings;
        adminCommonFactory.settings = _settings;
        return adminCommonFactory;

    }]);
