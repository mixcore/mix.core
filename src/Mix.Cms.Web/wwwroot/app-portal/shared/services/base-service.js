'use strict';
app.factory('BaseServices', ['$rootScope', 'CommonServices',
    function ($rootScope, commonServices) {
        var serviceFactory = {};
        
        var _init = function (modelName) {
            this.lang = $rootScope.configurationService.get('lang');
            this.prefixUrl = 'api/' + this.lang + '/' + modelName;
        }
        var _getSingle = async function (params = []) {
            var url = _prefixUrl + '/details';
            for (let i = 0; i < params.length; i++) {
                url += '/' + params[i];
            }
            var req = {
                method: 'GET',
                url: url
            };
            return await commonServices.getApiResult(req);
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

            return await commonServices.getApiResult(req);
        };

        var _remove = async function (id) {
            var url = this.prefixUrl + '/delete/'+ id; 
            var req = {
                method: 'GET',
                url: url
            };
            return await commonServices.getApiResult(req);
        };

        var _save = async function (objData) {
            var url = this.prefixUrl + '/save'; 
            var req = {
                method: 'POST',
                url: url,
                data: JSON.stringify(objData)
            };
            return await commonServices.getApiResult(req);
        };

        var _updateInfos = async function (objData) {
            var url = this.prefixUrl + '/update-infos'; 
            var req = {
                method: 'POST',
                url: url,
                data: JSON.stringify(objData)
            };
            return await commonServices.getApiResult(req);
        };
        serviceFactory.lang = '';
        serviceFactory.prefixUrl = '';
        serviceFactory.init = _init;
        serviceFactory.getSingle = _getSingle;
        serviceFactory.getList = _getList;
        serviceFactory.save = _save;
        serviceFactory.delete = _delete;
        serviceFactory.updateInfos = _updateInfos;
        return serviceFactory;

    }]);