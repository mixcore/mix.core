(function (angular) {
    app.component('headerNav', {
        templateUrl: '/app/app-portal/components/header-nav/headerNav.html',
        controller: ['$rootScope', '$location', 'CommonService', 'AuthService', 'TranslatorService', 'GlobalSettingsService',
            function ($rootScope, $location, commonService, authService, translatorService, GlobalSettingsService) {
                var ctrl = this;
                if (authService.authentication) {
                    ctrl.avatar = authService.authentication.avatar;
                }
                GlobalSettingsService.fillGlobalSettings().then(function (response) {
                    ctrl.settings = response;
                });
                ctrl.translate = $rootScope.translate;
                ctrl.getConfiguration = $rootScope.getConfiguration;
                ctrl.changeLang = function (lang, langIcon) {
                    ctrl.settings.lang = lang;
                    ctrl.settings.langIcon = langIcon;
                    commonService.fillSettings(lang).then(function () {
                        translatorService.reset(lang).then(function () {
                            GlobalSettingsService.reset(lang).then(function () {
                                window.top.location = location.href;
                            });
                        });
                    });
                };
                ctrl.logOut = function () {
                    $rootScope.logOut();
                };
                ctrl.addFavorite = function () {
                    $('#dlg-favorite').modal('show');
                }
            }],
        bindings: {
            breadCrumbs: '=',
            settings: '='
        }
    });
})(window.angular);