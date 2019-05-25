'use strict';
app.factory('Step2Services', ['$http', 'CommonService', function ($http, commonService) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var usersServiceFactory = {};
    var apiUrl = '/portal/';
    var _register = async function (user) {
        var apiUrl = '/account/register';
        var req = {
            method: 'POST',
            url: apiUrl,
            data: JSON.stringify(user)
        };

        return await commonService.getApiResult(req);
    };

    usersServiceFactory.register = _register;
    return usersServiceFactory;

}]);
