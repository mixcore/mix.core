'use strict';
app.factory('DashboardServices', ['$rootScope', '$http', 'CommonService', function ($rootScope, $http, commonService) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var usersServiceFactory = {};
    var apiUrl = '/portal/';
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
