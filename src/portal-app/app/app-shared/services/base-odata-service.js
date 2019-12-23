'use strict';
app.factory('BaseODataService', ['$rootScope', '$routeParams', 'CommonService',
    function ($rootScope, $routeParams, commonService) {
        var serviceFactory = {};

        var _init = function (modelName, isGlobal) {
            this.modelName = modelName;
            if (!isGlobal && isGlobal != 'true') {
                this.lang = $rootScope.settings.lang;
                this.prefixUrl = '/odata/' + this.lang + '/' + modelName;
            }
            else {
                this.prefixUrl = '/odata/' + modelName;
            }
        };

        var _getSingle = async function (viewType, params = []) {
            var url = this.prefixUrl + '/' + viewType;
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
        var _count = async function (viewType, params = []) {
            var url = this.prefixUrl + '/' + viewType + '/count';
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
        var _getList = async function (viewType, objData, params = []) {

            var data = serviceFactory.parseODataQuery(objData);
            var url = this.prefixUrl + '/' + viewType;
            for (let i = 0; i < params.length; i++) {
                if (params[i] != null) {
                    url += '/' + params[i];
                }
            }
            if (data) {
                url = url.concat(data);
            }
            var req = {
                method: 'GET',
                url: url
            };
            return await commonService.getApiResult(req);
        };

        var _delete = async function (params = []) {
            var url = this.prefixUrl + '/portal';
            for (let i = 0; i < params.length; i++) {
                if (params[i] != null) {
                    url += '/' + params[i];
                }
            }
            var req = {
                method: 'DELETE',
                url: url
            };
            return await commonService.getApiResult(req);
        };

        var _save = async function (viewType, objData) {
            var url = this.prefixUrl + '/' + viewType;
            var req = {
                method: 'POST',
                url: url,
                data: JSON.stringify(objData)
            };
            return await commonService.getApiResult(req);
        };

        var _saveFields = async function (viewType, id, objData) {
            var url = this.prefixUrl + '/' + viewType + '/' + id;
            var req = {
                method: 'POST',
                url: url,
                data: JSON.stringify(objData)
            };
            return await commonService.getApiResult(req);
        };
        var _saveProperties = async function (viewType, objData) {
            var url = this.prefixUrl + '/' + viewType + '/save-properties';
            var req = {
                method: 'POST',
                url: url,
                data: JSON.stringify(objData)
            };
            return await commonService.getApiResult(req);
        };

        var _saveList = async function (viewType, objData) {
            var url = this.prefixUrl + '/' + viewType + '/save-list';
            var req = {
                method: 'POST',
                url: url,
                data: JSON.stringify(objData)
            };
            return await commonService.getApiResult(req);
        };

        var _applyList = async function (viewType, objData) {
            var url = this.prefixUrl + '/' + viewType + '/apply-list';
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
                headers: { 'Content-Type': undefined },
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: form
            };
            return await commonService.getApiResult(req);
        };
        var _parseODataQuery = function (req) {
            if (req) {
                var skip = parseInt(req.pageIndex) * parseInt(req.pageSize);
                var top = parseInt(req.pageSize);
                var result = '?$skip=' + skip + '&$top=' + top + '&$orderby=' + req.orderBy;
                if (req.direction == '1') {
                    result += " desc";
                }
                if (req.filter) {
                    result += "&$filter=" + req.filter;
                }
                if (req.selects) {
                    result += "&$select=" + req.selects;
                }
                return result;
            }
            else {
                return '';
            }
        };

        serviceFactory.lang = '';
        serviceFactory.prefixUrl = '';
        serviceFactory.init = _init;
        serviceFactory.count = _count;
        serviceFactory.getSingle = _getSingle;
        serviceFactory.getList = _getList;
        serviceFactory.save = _save;
        serviceFactory.saveFields = _saveFields;
        serviceFactory.saveProperties = _saveProperties;
        serviceFactory.saveList = _saveList;
        serviceFactory.applyList = _applyList;
        serviceFactory.delete = _delete;
        serviceFactory.ajaxSubmitForm = _ajaxSubmitForm;
        serviceFactory.parseODataQuery = _parseODataQuery;
        return serviceFactory;

    }]);