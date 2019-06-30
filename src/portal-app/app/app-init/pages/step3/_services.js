'use strict';
app.factory('Step3Services', ['CommonService'
    , function (commonService) {

        //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

        var service = {};
        var _submit = async function (data) {
            var req = {
                method: 'POST',
                url: '/init/init-cms/step-3',
                data: JSON.stringify(data)
            };
            return await commonService.getApiResult(req);
        };
        service.submit = _submit;
        return service;

    }]);
