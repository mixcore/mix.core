'use strict';
app.factory('ThemeService', ['CommonService', 'BaseService',
    function (commonService, baseService) {

        var serviceFactory = Object.create(baseService);
        serviceFactory.init('theme');

        var _syncTemplates = async function (id) {
            var url = this.prefixUrl + '/sync/' + id;
            var req = {
                method: 'GET',
                url: url
            };
            return await commonService.getApiResult(req, '');
        };

        serviceFactory.syncTemplates = _syncTemplates;
        return serviceFactory;

    }]);
