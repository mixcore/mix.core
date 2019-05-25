'use strict';
app.factory('ModuleDataService', ['$rootScope', 'CommonService', 'BaseService',
    function ($rootScope, commonService, baseService) {

        var serviceFactory = Object.create(baseService);
        serviceFactory.init('module-data');
        var _delete = async function(moduleId, dataId){
            var url = this.prefixUrl + '/delete/' + moduleId+'/'+dataId;
            var req = {
                method: 'GET',
                url: url
            };
            return await commonService.getApiResult(req);
        }
        var _updateInfos = async function (modules) {

            var req = {
                method: 'POST',
                url: this.prefixUrl + '/update-infos',
                data: JSON.stringify(modules)
            };
            return await commonService.getApiResult(req);
        };
        serviceFactory.delete = _delete;
        serviceFactory.updateInfos = _updateInfos;
        return serviceFactory;

    }]);
