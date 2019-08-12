'use strict';
app.factory('PagePositionService', ['$rootScope', 'CommonService', 'BaseService',
    function ($rootScope, commonService, baseService) {

        var serviceFactory = Object.create(baseService);
        serviceFactory.init('page-position');
        var _delete = async function(pageId, positionId){
            var url = this.prefixUrl + '/delete/' + pageId+'/'+positionId;
            var req = {
                method: 'GET',
                url: url
            };
            return await commonService.getApiResult(req);
        }
        var _updateInfos = async function (pages) {

            var req = {
                method: 'POST',
                url: this.prefixUrl + '/update-infos',
                data: JSON.stringify(pages)
            };
            return await commonService.getApiResult(req);
        };
        serviceFactory.delete = _delete;
        serviceFactory.updateInfos = _updateInfos;
        return serviceFactory;

    }]);
