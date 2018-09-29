'use strict';
app.factory('Step2Services', ['$http', 'CommonService', function ($http, commonServices) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var usersServiceFactory = {};
    var apiUrl = '/api/portal/';
    var _register = async function (user) {
        var apiUrl = '/api/account/register';
        var req = {
            method: 'POST',
            url: apiUrl,
            data: JSON.stringify(user)
        };

        return await commonServices.getApiResult(req);
    };

    usersServiceFactory.register = _register;
    return usersServiceFactory;

}]);
