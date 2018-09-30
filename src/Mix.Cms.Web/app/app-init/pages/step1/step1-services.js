'use strict';
app.factory('Step1Services', ['$http', 'CommonService', function ($http, commonServices) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';
    
    var step1ServiceFactory = {};
    var apiUrl = '/portal/';
    var _initCms = async function (data) {
        var req = {
            method: 'POST',
            url: '/portal/init-cms',
            data: JSON.stringify(data)
        };
        return await commonServices.getApiResult(req);
    };

    step1ServiceFactory.initCms = _initCms;
    return step1ServiceFactory;

}]);
