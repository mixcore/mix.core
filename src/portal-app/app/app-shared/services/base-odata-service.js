'use strict';
app.factory('BaseODataService', ['$rootScope', '$routeParams', 'CommonService',
    function ($rootScope, $routeParams, commonService) {
        var serviceFactory = {};

        var _init = function (modelName, viewType, isGlobal) {
            this.modelName = modelName;
            if(!isGlobal)
            {
                this.lang = $rootScope.settings.lang;
                this.prefixUrl = '/odata/' + this.lang + '/' + modelName + '/' + viewType;
            }
            else{
                this.prefixUrl = '/odata/' + modelName + '/' + viewType;
            }
        };

        var _getSingle = async function (params = []) {
            var url = this.prefixUrl;
            for (let i = 0; i < params.length; i++) {
                if (params[i] != null) {
                    url += '/' + params[i];
                }
            }
            var req = {
                method: 'GET',
                url: url
            };
            return await commonService.getApiResult(req);
        };
        var _count = async function (params = []) {
            var url = this.prefixUrl;
            for (let i = 0; i < params.length; i++) {
                if (params[i] != null) {
                    url += '/' + params[i];
                }
            }
            var req = {
                method: 'GET',
                url: url + '/count'
            };
            return await commonService.getApiResult(req);
        };
        var _getList = async function (objData) {
                               
            var data = _parseODataQuery(objData);
            var url = this.prefixUrl.concat(data);
            var req = {
                method: 'GET',
                url: url
            };
            return await commonService.getApiResult(req);
        };

        var _delete = async function (id) {
            var url = this.prefixUrl + '/' + id;
            var req = {
                method: 'DELETE',
                url: url
            };
            return await commonService.getApiResult(req);
        };

        var _save = async function (objData) {
            var url = this.prefixUrl;
            var req = {
                method: 'POST',
                url: url,
                data: JSON.stringify(objData)
            };
            return await commonService.getApiResult(req);
        };
       
        var _saveFields = async function (id, objData) {
            var url = this.prefixUrl + '/' + id;
            var req = {
                method: 'POST',
                url: url,
                data: JSON.stringify(objData)
            };
            return await commonService.getApiResult(req);
        };

        var _saveList = async function (objData) {
            var url = this.prefixUrl + '/save-list';
            var req = {
                method: 'POST',
                url: url,
                data: JSON.stringify(objData)
            };
            return await commonService.getApiResult(req);
        };

       
        var _ajaxSubmitForm = async function (form, url) {            
            var req = {
                method: 'POST',
                url: url,
                headers: {'Content-Type': undefined},
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: form
            };
            return await commonService.getApiResult(req);            
        };
        var _parseODataQuery = function(req){
            var skip = parseInt(req.pageIndex) * parseInt(req.pageSize);
            var top = parseInt(req.pageSize);
            var result = '?$skip=' + skip + '&$top=' + top + '&$orderby=' + req.orderBy;
            if (req.selects){
                result += "&$select="+req.selects;
            }
            return result;
        };

        serviceFactory.lang = '';
        serviceFactory.prefixUrl = '';
        serviceFactory.init = _init;
        serviceFactory.count = _count;
        serviceFactory.getSingle = _getSingle;
        serviceFactory.getList = _getList;
        serviceFactory.save = _save;
        serviceFactory.saveFields = _saveFields;
        serviceFactory.saveList = _saveList;
        serviceFactory.delete = _delete;
        serviceFactory.ajaxSubmitForm = _ajaxSubmitForm;
        return serviceFactory;

    }]);