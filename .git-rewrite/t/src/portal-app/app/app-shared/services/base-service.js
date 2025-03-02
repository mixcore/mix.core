'use strict';
app.factory('BaseService', ['$rootScope', '$routeParams', 'CommonService',
    function ($rootScope, $routeParams, commonService) {
        var serviceFactory = {};

        var _init = function (modelName, isGlobal) {
            this.modelName = modelName;
            if(!isGlobal)
            {
                this.lang = $rootScope.settings.lang;
                this.prefixUrl = '/' + this.lang + '/' + modelName;
            }
            else{
                this.prefixUrl = '/' + modelName;
            }
        }
        var _getSingle = async function (params = []) {
            var url = (this.prefixUrl || '/' + this.lang + '/' + this.modelName) + '/details';
            for (let i = 0; i < params.length; i++) {
                if (params[i]) {
                    url += '/' + params[i];
                }
            }
            var req = {
                method: 'GET',
                url: url
            };
            return await commonService.getApiResult(req);
        };
        var _getList = async function (objData, params = []) {
            var url = this.prefixUrl + '/list';
            for (let i = 0; i < params.length; i++) {
                url += '/' + params[i];
            }
            var req = {
                method: 'POST',
                url: url,
                data: JSON.stringify(objData)
            };

            return await commonService.getApiResult(req);
        };

        var _delete = async function (id) {
            var url = this.prefixUrl + '/delete/' + id;
            var req = {
                method: 'GET',
                url: url
            };
            return await commonService.getApiResult(req);
        };

        var _save = async function (objData) {
            var url = this.prefixUrl + '/save';
            var req = {
                method: 'POST',
                url: url,
                data: JSON.stringify(objData)
            };
            return await commonService.getApiResult(req);
        };
        var _applyList = async function (objData) {
            var url = this.prefixUrl + '/apply-list';
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

        var _updateInfos = async function (objData) {
            var url = this.prefixUrl + '/update-infos';
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
        serviceFactory.lang = '';
        serviceFactory.prefixUrl = '';
        serviceFactory.init = _init;
        serviceFactory.getSingle = _getSingle;
        serviceFactory.getList = _getList;
        serviceFactory.save = _save;
        serviceFactory.applyList = _applyList;
        serviceFactory.saveList = _saveList;
        serviceFactory.delete = _delete;
        serviceFactory.updateInfos = _updateInfos;
        serviceFactory.ajaxSubmitForm = _ajaxSubmitForm;
        return serviceFactory;

    }]);