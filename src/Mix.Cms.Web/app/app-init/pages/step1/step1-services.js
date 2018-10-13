'use strict';
app.factory('Step1Services', ['$http', 'CommonService', function ($http, commonService) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';
    
    var step1ServiceFactory = {};
    var apiUrl = '/portal/';
    var _initCms = async function (data) {
        var req = {
            method: 'POST',
            url: '/portal/init-cms',
            data: JSON.stringify(data)
        };
        return await commonService.getApiResult(req);
    };

    step1ServiceFactory.initCms = _initCms;
    return step1ServiceFactory;

}]);
