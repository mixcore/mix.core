'use strict';
app.factory('TranslatorService', ['$rootScope', 'CommonService', 'localStorageService', function ($rootScope, commonService, localStorageService) {
    var factory = {};
    var _translator = {
        lang: '',
        data: null
    };
    var _init = function (translator) {
        this._translator = translator;
    };
    var _fillTranslator = async function (culture) {
        this.translator = localStorageService.get('translator');
        if (this.translator && this.translator.data && this.translator.lang === culture) {
            return this.translator;
        }
        else {

            this.translator = await _getTranslator(culture);
            return this.translator;
        }

    };
    var _getTranslator = async function (culture) {
        var translator = localStorageService.get('translator');
        if (translator && translator.lang === culture) {
            translator = translator;
            return translator;
        }
        else {
            translator = { lang: culture, data: null };
            var url = '/portal';
            if (culture) {
                url += '/' + culture;
            }
            url += '/translator';
            var req = {
                method: 'GET',
                url: url
            };
            translator.lang = culture;
            var getData = await commonService.getApiResult(req);
            if (getData.isSucceed) {
                translator.data = getData.data;
                localStorageService.set('translator', translator);
            }
            return translator;
        }
    };
    var _reset = async function (lang) {
        localStorageService.remove('translator');
        await _getTranslator(lang);
    };
    var _get = function (keyword, isWrap, defaultText) {
        if (!this.translator.data && $rootScope.globalSettings) {
            $rootScope.isBusy = true;
            this.fillTranslator($rootScope.globalSettings.lang).then(function (response) {
                $rootScope.isBusy = false;
                return response.data[keyword] || defaultText || getLinkCreateLanguage(keyword, isWrap);
            });
        } else {
            return this.translator.data[keyword] || defaultText || getLinkCreateLanguage(keyword, isWrap);
        }

    };

    var _getAsync = async function (keyword, defaultText) {
        if (!this.translator.data && $rootScope.globalSettings) {
            $rootScope.isBusy = true;
            this.translator = await _fillTranslator(lang);
            return this.translator.data[keyword] || defaultText || getLinkCreateLanguage(keyword, isWrap);
        } else {
            return this.translator.data[keyword] || defaultText || getLinkCreateLanguage(keyword, isWrap);
        }

    };

    var getLinkCreateLanguage = function (keyword, isWrap) {
        //return '<span data-key="/portal/language/details?k=' + keyword + '">[' + keyword + ']</span>';
        return isWrap ? '[' + keyword + ']' : keyword;
    };

    factory.getAsync = _getAsync;
    factory.get = _get;
    factory.init = _init;
    factory.reset = _reset;
    factory.translator = _translator;
    factory.fillTranslator = _fillTranslator;
    return factory;
}]);
