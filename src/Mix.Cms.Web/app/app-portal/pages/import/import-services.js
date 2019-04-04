'use strict';
app.factory('ImportFileServices', ['$rootScope', 'BaseService',
     function ($rootScope, baseService) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var serviceFactory = {};

    var settings = $rootScope.globalSettings;
    var serviceFactory = Object.create(baseService);
    serviceFactory.init('portal',true);
    var _saveImportFile = async function (frm) {
        var apiUrl = this.prefixUrl + '/' + settings.lang + '/import';
        return await this.ajaxSubmitForm(frm, apiUrl);
    };

    serviceFactory.saveImportFile = _saveImportFile;
    return serviceFactory;

}]);
