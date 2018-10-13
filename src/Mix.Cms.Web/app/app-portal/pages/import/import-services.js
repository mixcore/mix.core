'use strict';
app.factory('ImportFileServices', ['$http', '$rootScope', 'CommonService', function ($http, $rootScope, commonService) {

    //var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';

    var importFilesServiceFactory = {};

    var settings = $rootScope.globalSettings;

    var _saveImportFile = async function (importFile, type) {
        var apiUrl = '/portal/' + settings.lang + '/import/' + type;
        var req = {
            method: 'POST',
            url: apiUrl,
            data: JSON.stringify(importFile)
        };
        return await commonService.getApiResult(req);
    };

    importFilesServiceFactory.saveImportFile = _saveImportFile;
    return importFilesServiceFactory;

}]);
