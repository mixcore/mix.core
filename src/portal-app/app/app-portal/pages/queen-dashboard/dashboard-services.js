'use strict';
app.factory('QueenDashboardServices', ['$http', 'CommonService', function ($http, commonService) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var usersServiceFactory = {};
    var apiUrl = '/en-us/queen-beauty/';
    var _getDashboardInfo = async function () {
        var req = {
            method: 'GET',
            url: apiUrl + 'dashboard'
        };

        return await commonService.getApiResult(req);
    };

    usersServiceFactory.getDashboardInfo = _getDashboardInfo;
    return usersServiceFactory;

}]);
