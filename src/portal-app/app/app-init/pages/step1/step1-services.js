'use strict';
app.factory('Step1Services', ['$http', '$rootScope', 'AppSettings', 'CommonService'
    , function ($http, $rootScope, appSettings, commonService) {

        var step1ServiceFactory = {};
        var _saveDefaultSettings = async function () {
            var req = {
                method: 'GET',
                url: '/portal/app-settings/save-default'
            };
            return commonService.getAnonymousApiResult(req);
        };
        
        var _initCms = async function (data) {
            var req = {
                method: 'POST',
                url: '/init/init-cms/step-1',
                data: JSON.stringify(data)
            };
            return await commonService.getAnonymousApiResult(req);
        };
        
        step1ServiceFactory.initCms = _initCms;
        step1ServiceFactory.saveDefaultSettings = _saveDefaultSettings;
        return step1ServiceFactory;

    }]);
