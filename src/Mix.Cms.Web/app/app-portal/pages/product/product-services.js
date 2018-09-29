'use strict';
app.factory('ProductServices', ['$http', '$rootScope', 'CommonService', function ($http, $rootScope, commonServices) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var productsServiceFactory = {};

    var settings = $rootScope.globalSettings

    var _getProduct = async function (id, type) {
        var apiUrl = '/' + settings.lang + '/product/';
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

    var _initProduct = async function (type) {
        var apiUrl = '/' + settings.lang + '/product/';
        var req = {
            method: 'GET',
            url: apiUrl + 'init/' + type,
        };
        return await commonServices.getApiResult(req)
    };

    var _getProducts = async function (request) {
        var apiUrl = '/' + settings.lang + '/product/';
        var req = {
            method: 'POST',
            url: apiUrl + 'list',
            data: JSON.stringify(request)
        };
        
        return await commonServices.getApiResult(req);
    };

    var _removeProduct = async function (id) {
        var apiUrl = '/' + settings.lang + '/product/';
        var req = {
            method: 'GET',
            url: apiUrl + 'delete/' + id
        };
        return await commonServices.getApiResult(req)
    };

    var _saveProduct = async function (product) {
        var apiUrl = '/' + settings.lang + '/product/';
        var req = {
            method: 'POST',
            url: apiUrl + 'save',
            data: JSON.stringify(product)
        };
        return await commonServices.getApiResult(req)
    };

    productsServiceFactory.getProduct = _getProduct;
    productsServiceFactory.initProduct = _initProduct;
    productsServiceFactory.getProducts = _getProducts;
    productsServiceFactory.removeProduct = _removeProduct;
    productsServiceFactory.saveProduct = _saveProduct;
    return productsServiceFactory;

}]);
