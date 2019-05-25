'use strict';
app.factory('CustomerServices', ['$http', '$rootScope', 'CommonService', function ($http, $rootScope, commonService) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var customersServiceFactory = {};

    var settings = $rootScope.globalSettings

    var _getCustomer = async function (id, type) {
        var apiUrl = '/queen-beauty/customer/';
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

    var _initCustomer = async function (type) {
        var apiUrl = '/queen-beauty/customer/';
        var req = {
            method: 'GET',
            url: apiUrl + 'init/' + type,
        };
        return await commonService.getApiResult(req)
    };

    var _getCustomers = async function (request) {
        var apiUrl = '/queen-beauty/customer/';
        var req = {
            method: 'POST',
            url: apiUrl + 'list',
            data: JSON.stringify(request)
        };
        
        return await commonService.getApiResult(req);
    };

    var _removeCustomer = async function (id) {
        var apiUrl = '/queen-beauty/customer/';
        var req = {
            method: 'GET',
            url: apiUrl + 'delete/' + id
        };
        return await commonService.getApiResult(req)
    };

    var _saveCustomer = async function (customer) {
        var apiUrl = '/queen-beauty/customer/';
        var req = {
            method: 'POST',
            url: apiUrl + 'save',
            data: JSON.stringify(customer)
        };
        return await commonService.getApiResult(req)
    };

    customersServiceFactory.getCustomer = _getCustomer;
    customersServiceFactory.initCustomer = _initCustomer;
    customersServiceFactory.getCustomers = _getCustomers;
    customersServiceFactory.removeCustomer = _removeCustomer;
    customersServiceFactory.saveCustomer = _saveCustomer;
    return customersServiceFactory;

}]);
