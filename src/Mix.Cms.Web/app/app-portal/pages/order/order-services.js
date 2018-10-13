'use strict';
app.factory('OrderServices', ['$http', '$rootScope', 'CommonService', function ($http, $rootScope, commonService) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var ordersServiceFactory = {};

    var settings = $rootScope.globalSettings

    var _getOrder = async function (id, type) {
        var apiUrl = '/' + settings.lang + '/order/';
        var url = apiUrl + 'details/' + type;
        if (id) {
            url += '/' + id;
        }
        var req = {
            method: 'GET',
            url: url
        };
        return await commonService.getApiResult(req)
    };

    var _initOrder = async function (type) {
        var apiUrl = '/' + settings.lang + '/order/';
        var req = {
            method: 'GET',
            url: apiUrl + 'init/' + type,
        };
        return await commonService.getApiResult(req)
    };

    var _getOrders = async function (request) {
        var apiUrl = '/' + settings.lang + '/order/';
        var req = {
            method: 'POST',
            url: apiUrl + 'list',
            data: JSON.stringify(request)
        };
        
        return await commonService.getApiResult(req);
    };

    var _removeOrder = async function (id) {
        var apiUrl = '/' + settings.lang + '/order/';
        var req = {
            method: 'GET',
            url: apiUrl + 'delete/' + id
        };
        return await commonService.getApiResult(req)
    };

    var _saveOrder = async function (order) {
        var apiUrl = '/' + settings.lang + '/order/';
        var req = {
            method: 'POST',
            url: apiUrl + 'save',
            data: JSON.stringify(order)
        };
        return await commonService.getApiResult(req)
    };

    ordersServiceFactory.getOrder = _getOrder;
    ordersServiceFactory.initOrder = _initOrder;
    ordersServiceFactory.getOrders = _getOrders;
    ordersServiceFactory.removeOrder = _removeOrder;
    ordersServiceFactory.saveOrder = _saveOrder;
    return ordersServiceFactory;

}]);
