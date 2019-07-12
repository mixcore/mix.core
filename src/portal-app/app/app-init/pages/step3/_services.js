'use strict';
app.factory('Step3Services', ['CommonService'
    , function (commonService) {

        var service = {};
        var _submit = async function (data) {
            var req = {
                method: 'POST',
                url: '/init/init-cms/step-3',
                data: JSON.stringify(data)
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
        service.submit = _submit;
        service.ajaxSubmitForm = _ajaxSubmitForm;
        return service;

    }]);
