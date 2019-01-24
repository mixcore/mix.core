(function (angular) {
    app.component('headerNav', {
        templateUrl: '/app/app-portal/components/header-nav/headerNav.html',
        controller: ['$rootScope', '$location', 'CommonService', 'AuthService', 'TranslatorService', 'GlobalSettingsService',
            function ($rootScope, $location, commonService, authService, translatorService, globalSettingsService) {
                var ctrl = this;
                ctrl.globalSettings = $rootScope.globalSettings;
                if (authService.authentication) {
                    ctrl.avatar = authService.authentication.avatar;
                }
                globalSettingsService.fillGlobalSettings().then(function (response) {
                    ctrl.settings = response;
                });
                ctrl.translate = $rootScope.translate;
                ctrl.getConfiguration = function (keyword, isWrap, defaultText) {
                    return  $rootScope.getConfiguration(keyword, isWrap, defaultText);
                }
                ctrl.changeLang = function (lang, langIcon) {
                    ctrl.settings.lang = lang;
                    ctrl.settings.langIcon = langIcon;
                    commonService.fillSettings(lang).then(function () {
                        translatorService.reset(lang).then(function () {
                            globalSettingsService.reset(lang).then(function () {
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
                ctrl.toggleSidebar = function () {           
                    $('#sidebar').toggleClass('active');
                }
                ctrl.generateSitemap = async function(){
                    $rootScope.isBusy = true;
                    var resp = await commonService.genrateSitemap();
                    if(resp)
                    {
                        window.top.location.href = '/portal/file/details?folder=' + resp.fileFolder + '&filename=' + resp.fileName + resp.extension;
                    }
                    else{
                        $rootScope.isBusy = false;
                        $rootScope.showErrors(['Server error']);
                    }
                }
            }],
        bindings: {
            breadCrumbs: '=',
            settings: '='
        }
    });
})(window.angular);