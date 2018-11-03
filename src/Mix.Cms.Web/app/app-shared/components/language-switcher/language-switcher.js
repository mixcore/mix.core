(function (angular) {
    app.component('languageSwitcher', {
        templateUrl: '/app/app-shared/components/language-switcher/language-switcher.html',
        controller: ['$rootScope', '$location', 'CommonService', 'TranslatorService', function ($rootScope, $location, commonService, translatorService) {
            var ctrl = this;
            ctrl.changeLang = async function (lang, langIcon) {
                var oldLang = ctrl.settings.lang;
                ctrl.settings.lang = lang;
                ctrl.settings.langIcon = langIcon;
                await commonService.removeSettings();
                await commonService.removeTranslator();
                commonService.fillSettings(lang).then(function () {
                    translatorService.reset(lang).then(function () {
                        var url = $location.$$absUrl;
                        window.top.location = url.replace(oldLang, lang);
                    });
                });
            };
            ctrl.logOut = function () {
                $rootScope.logOut();
            };
        }],
        bindings: {
            settings: '=',
            ulStyle: '=',
            liStyle: '=',
            aStyle: '=',
            activeClass: '='
        }
    });
})(window.angular);