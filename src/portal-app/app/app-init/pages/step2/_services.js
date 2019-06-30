'use strict';
app.factory('Step2Services', ['$http', 'CommonService', function ($http, commonService) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var usersServiceFactory = {};
    var _register = async function (user) {
        var req = {
            method: 'POST',
            url: '/init/init-cms/step-2',
            data: JSON.stringify(user)
        };

        return await commonService.getApiResult(req);
    };

    usersServiceFactory.register = _register;
    return usersServiceFactory;

}]);
