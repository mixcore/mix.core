'use strict';
app.factory('AppSettingsServices', ['$http', '$rootScope', 'CommonService', function ($http, $rootScope, commonService) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var appSettingssServiceFactory = {};

    var settings = $rootScope.globalSettings;

    var _getAppSettings = async function () {
        var url = '/portal/app-settings/details';
        
        var req = {
            method: 'GET',
            url: url
        };
        return await commonService.getApiResult(req);
    };

    var _saveAppSettings = async function (appSettings) {
        var apiUrl = '/portal/app-settings/save';
        var req = {
            method: 'POST',
            url: apiUrl,
            data: JSON.stringify(appSettings)
        };
        return await commonService.getApiResult(req);
    };

    appSettingssServiceFactory.getAppSettings = _getAppSettings;
    appSettingssServiceFactory.saveAppSettings = _saveAppSettings;
    return appSettingssServiceFactory;

}]);
