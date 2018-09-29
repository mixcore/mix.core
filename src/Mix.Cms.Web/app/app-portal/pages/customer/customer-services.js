'use strict';
app.factory('CustomerServices', ['$http', '$rootScope', 'CommonService', function ($http, $rootScope, commonServices) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var customersServiceFactory = {};

    var settings = $rootScope.globalSettings

    var _getCustomer = async function (id, type) {
        var apiUrl = '/api/queen-beauty/customer/';
        var url = apiUrl + 'details/' + type;
        if (id) {
            url += '/' + id;
        }
        var req = {
            method: 'GET',
            url: url
        };
        return await commonServices.getApiResult(req)
    };

    var _initCustomer = async function (type) {
        var apiUrl = '/api/queen-beauty/customer/';
        var req = {
            method: 'GET',
            url: apiUrl + 'init/' + type,
        };
        return await commonServices.getApiResult(req)
    };

    var _getCustomers = async function (request) {
        var apiUrl = '/api/queen-beauty/customer/';
        var req = {
            method: 'POST',
            url: apiUrl + 'list',
            data: JSON.stringify(request)
        };
        
        return await commonServices.getApiResult(req);
    };

    var _removeCustomer = async function (id) {
        var apiUrl = '/api/queen-beauty/customer/';
        var req = {
            method: 'GET',
            url: apiUrl + 'delete/' + id
        };
        return await commonServices.getApiResult(req)
    };

    var _saveCustomer = async function (customer) {
        var apiUrl = '/api/queen-beauty/customer/';
        var req = {
            method: 'POST',
            url: apiUrl + 'save',
            data: JSON.stringify(customer)
        };
        return await commonServices.getApiResult(req)
    };

    customersServiceFactory.getCustomer = _getCustomer;
    customersServiceFactory.initCustomer = _initCustomer;
    customersServiceFactory.getCustomers = _getCustomers;
    customersServiceFactory.removeCustomer = _removeCustomer;
    customersServiceFactory.saveCustomer = _saveCustomer;
    return customersServiceFactory;

}]);
