'use strict';
app.factory('CultureService', ['BaseService', function (baseService) {

    var serviceFactory = Object.create(baseService);
    serviceFactory.init('culture');
    var _updateInfos = async function (pages) {

        var req = {
            method: 'POST',
            url: this.prefixUrl + '/update-infos',
            data: JSON.stringify(pages)
        };
        return await commonService.getApiResult(req);
    };

    var _syncTemplates = async function (id) {
        var apiUrl = '/culture/';
        var url = apiUrl + 'sync/' + id;        
        var req = {
            method: 'GET',
            url: url
        };
        return await commonService.getApiResult(req)
    };

    serviceFactory.syncTemplates = _syncTemplates;
    serviceFactory.updateInfos = _updateInfos;
    return serviceFactory;

}]);
