'use strict';
app.factory('CommonService', ['$location', '$http', '$rootScope', 'AuthService', 'localStorageService', 'AppSettings',
    function ($location, $http, $rootScope, authService, localStorageService, appSettings) {
        var adminCommonFactory = {};
        var _settings = {
            lang: '',
            cultures: []
        };
        var _loadJArrayData = async function (name) {
            var req = {
                method: 'GET',
                url: '/portal/jarray-data/' + name
            };
            return await _getAnonymousApiResult(req);
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

        var _sendMail = async function (subject, body) {
            var url = '/portal/sendmail';
            var req = {
                method: 'POST',
                url: url,
                data: { subject: subject, body: body }
            };
            return _getApiResult(req).then(function (response) {
                return response.data;
            });
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
                url += '/settings';
                var req = {
                    method: 'GET',
                    url: url
                };
                return _getApiResult(req).then(function (response) {
                    return response.data;
                });
            }
        };
        var _getAllSettings = async function (culture) {
            var settings = localStorageService.get('settings');
            var globalSettings = localStorageService.get('globalSettings');
            var translator = localStorageService.get('translator');
            if (settings && globalSettings && translator && settings.lang === culture) {
                $rootScope.settings = settings;
                $rootScope.globalSettings = globalSettings;
                $rootScope.translator.translator = translator;
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
                    localStorageService.set('settings', response.data.settings);
                    localStorageService.set('globalSettings', response.data.globalSettings);
                    localStorageService.set('translator', response.data.translator);
                    $rootScope.settings = response.data.settings;
                    $rootScope.globalSettings = response.data.globalSettings;
                    $rootScope.translator.translator = response.data.translator;
                });
            }
        };

        var _checkConfig = async function (lastSync) {
            if (lastSync) {
                var url = '/portal/check-config/' + lastSync;
                var req = {
                    method: 'GET',
                    url: url
                };
                return _getApiResult(req).then(function (response) {
                    if (response.data) {
                        _removeSettings().then(
                            () => {
                                _removeTranslator().then(() => {
                                    localStorageService.set('settings', response.data.settings);
                                    localStorageService.set('globalSettings', response.data.globalSettings);
                                    localStorageService.set('translator', response.data.translator);
                                    $rootScope.settings = response.data.settings;
                                    $rootScope.globalSettings = response.data.globalSettings;
                                    $rootScope.translator.translator = response.data.translator;
                                });
                            }
                        );
                    }
                    else {
                        $rootScope.settings = localStorageService.get('settings');
                        $rootScope.globalSettings = localStorageService.get('globalSettings');
                        $rootScope.translator.translator = localStorageService.get('translator');
                    }
                });
            }
        };

        var _genrateSitemap = async function () {

            var url = '/portal';
            url += '/sitemap';
            var req = {
                method: 'GET',
                url: url
            };
            return _getApiResult(req).then(function (response) {
                return response.data;
            });
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
                if (culture && settings && settings.lang !== culture) {
                    await _removeSettings();
                    await _removeTranslator();
                }
                settings = await _getSettings(culture);
                localStorageService.set('settings', settings);
                //window.top.location = location.href;
                return settings;
            }

        };
        var _fillAllSettings = async function (culture) {
            var settings = localStorageService.get('settings');
            var globalSettings = localStorageService.get('globalSettings');
            var translator = localStorageService.get('translator');
            if (settings && globalSettings && translator && (!culture || settings.lang === culture)) {
                $rootScope.settings = settings;
                $rootScope.globalSettings = globalSettings;
                $rootScope.translator.translator = translator;
                await _checkConfig(globalSettings.lastUpdateConfiguration);
            }
            else {
                if (culture && settings && settings.lang !== culture) {
                    await _removeSettings();
                    await _removeTranslator();
                }
                await _getAllSettings(culture);
            }

        };
        var _getApiResult = async function (req, serviceBase) {
            //console.log(req.data);
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
                if (req.url.indexOf('settings') == -1 &&
                    (!$rootScope.settings ||
                        $rootScope.settings.lastUpdateConfiguration < resp.data.lastUpdateConfiguration)
                ) {
                    _initAllSettings();
                }

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
                                window.top.location.href = '/security/login';
                            });
                        }, function (err) {

                            var t = { isSucceed: false, errors: [err.status] };

                            authService.logOut();
                            authService.authentication.token = null;
                            authService.authentication.refresh_token = null;
                            authService.referredUrl = $location.$$url;
                            window.top.location.href = '/security/login';
                            return t;
                        }
                        );
                    }
                    else if (error.status === 403) {
                        var t = { isSucceed: false, errors: ['Forbidden'] };
                        window.top.location.href = '/security/login';
                        return t;
                    }
                    else {
                        return { isSucceed: false, errors: [error.statusText || error.status] };
                    }
                });
        };

        var _getAnonymousApiResult = async function (req) {
            $rootScope.isBusy = true;
            var serviceUrl = appSettings.serviceBase + '/api/' + appSettings.apiVersion;
            req.url = serviceUrl + req.url;
            req.headers = {
                'Content-Type': 'application/json'
            };
            return $http(req).then(function (resp) {
                return resp.data;
            },
                function (error) {
                    return { isSucceed: false, errors: [error.statusText || error.status] };
                });
        };
        adminCommonFactory.sendMail = _sendMail;
        adminCommonFactory.getApiResult = _getApiResult;
        adminCommonFactory.getAnonymousApiResult = _getAnonymousApiResult;
        adminCommonFactory.getSettings = _getSettings;
        adminCommonFactory.setSettings = _setSettings;
        adminCommonFactory.initAllSettings = _initAllSettings;
        adminCommonFactory.fillAllSettings = _fillAllSettings;
        adminCommonFactory.removeSettings = _removeSettings;
        adminCommonFactory.removeTranslator = _removeTranslator;
        adminCommonFactory.showAlertMsg = _showAlertMsg;
        adminCommonFactory.checkfile = _checkfile;
        adminCommonFactory.fillSettings = _fillSettings;
        adminCommonFactory.settings = _settings;
        adminCommonFactory.genrateSitemap = _genrateSitemap;
        adminCommonFactory.loadJArrayData = _loadJArrayData;
        return adminCommonFactory;

    }]);

