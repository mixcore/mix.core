'use strict';
app.factory('Step1Services', ['$http', '$rootScope', 'AppSettings'
    , function ($http, $rootScope, appSettings) {

        //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

        var step1ServiceFactory = {};
        var apiUrl = '/portal/';
        var _saveDefaultSettings = async function () {
            var req = {
                method: 'GET',
                url: '/portal/app-settings/save-default'
            };
            return await _getApiResult(req);
        };
        var _initCms = async function (data) {
            var req = {
                method: 'POST',
                url: '/portal/init-cms',
                data: JSON.stringify(data)
            };
            return await _getApiResult(req);
        };
        var _getApiResult = async function (req) {
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
        step1ServiceFactory.initCms = _initCms;
        step1ServiceFactory.saveDefaultSettings = _saveDefaultSettings;
        return step1ServiceFactory;

    }]);
