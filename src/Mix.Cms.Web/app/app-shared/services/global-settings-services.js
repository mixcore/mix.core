'use strict';
app.factory('GlobalSettingsService', ['$rootScope', 'CommonService', 'localStorageService', 'AppSettings',
    function ($rootScope, commonService, localStorageService, appSettings) {
        var factory = {};
        var _globalSettings = {
            lang: '',
            data: null
        };
        var _fillGlobalSettings = async function (culture) {
            this.globalSettings = localStorageService.get('globalSettings');
            if (this.globalSettings && this.globalSettings.data && this.globalSettings.lang === culture) {

                //_globalSettings = globalSettings;
                //this.globalSettings = globalSettings;

                return this.globalSettings;
            }
            else {

                this.globalSettings = await _getglobalSettings(culture);
                return this.globalSettings;
            }

        };
        var _getglobalSettings = async function (culture) {
            var globalSettings = localStorageService.get('globalSettings');
            if (globalSettings && (!culture || globalSettings.lang === culture)) {
                globalSettings = globalSettings;
                return globalSettings;
            }
            else {
                globalSettings = { lang: culture, data: null };
                var url = '/portal';
                if (culture) {
                    url += '/' + culture;
                }
                url += '/global-settings';
                var req = {
                    method: 'GET',
                    url: url
                };
                var getData = await commonService.getApiResult(req);
                if (getData.isSucceed) {
                    globalSettings = getData.data;
                    localStorageService.set('globalSettings', globalSettings);
                }
                return globalSettings;
            }
        };
        var _reset = async function (lang) {
            localStorageService.remove('globalSettings');
            await _getglobalSettings(lang);
        };
        var _get = function (keyword, isWrap, defaultText) {
            if (!this.globalSettings && $rootScope.globalSettings) {
                $rootScope.isBusy = true;
                this.fillGlobalSettings($rootScope.globalSettings.lang).then(function (response) {
                    $rootScope.isBusy = false;
                    return response[keyword] || defaultText || getLinkCreateLanguage(keyword, isWrap);
                });
            } else {
                return this.globalSettings[keyword] || defaultText || getLinkCreateLanguage(keyword, isWrap);
            }

        };

        var _getAsync = async function (keyword, defaultText) {
            if (!this.globalSettings && $rootScope.globalSettings) {
                $rootScope.isBusy = true;
                this.globalSettings = await _fillGlobalSettings(lang);
                return this.globalSettings[keyword] || defaultText || getLinkCreateLanguage(keyword, isWrap);
            } else {
                return this.globalSettings[keyword] || defaultText || getLinkCreateLanguage(keyword, isWrap);
            }

        };

        var getLinkCreateLanguage = function (keyword, isWrap) {
            //return '<span data-key="/portal/language/details?k=' + keyword + '">[' + keyword + ']</span>';
            return isWrap ? '[' + keyword + ']' : keyword;
        };

        factory.getAsync = _getAsync;
        factory.get = _get;
        factory.reset = _reset;
        factory.globalSettings = _globalSettings;
        factory.fillGlobalSettings = _fillGlobalSettings;
        return factory;
    }]);
