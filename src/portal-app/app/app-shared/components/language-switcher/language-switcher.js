(function (angular) {
    app.component('languageSwitcher', {
        templateUrl: '/app/app-shared/components/language-switcher/language-switcher.html',
        controller: ['$rootScope','$scope', '$location',
            'CommonService', 'TranslatorService', 'GlobalSettingsService',
            function ($rootScope, $scope, $location, 
                commonService, translatorService, globalSettingsService) {
                var ctrl = this;
                ctrl.settings = {};
                this.$onInit = function(){
                        ctrl.settings = $rootScope.settings;
                        ctrl.settings.cultures = $rootScope.globalSettings.cultures;    
                };
                
                ctrl.changeLang = async function (lang, langIcon) {
                    ctrl.settings.lang = lang;
                    ctrl.settings.langIcon = langIcon;
                    // await commonService.removeSettings();
                    // await commonService.removeTranslator();
                    // commonService.fillSettings(lang).then(function () {
                    //     translatorService.reset(lang).then(function () {
                        commonService.fillAllSettings(lang).then(function (response) {
                            translatorService.translateUrl(lang).then(function(url){
                                window.top.location = url;
                            });
                        });
                    //     });
                    // });
                };
                ctrl.logOut = function () {
                    $rootScope.logOut();
                };
            }],
        bindings: {
            //settings: '=',
            ulStyle: '=',
            liStyle: '=',
            aStyle: '=',
            activeClass: '='
        }
    });
})(window.angular);