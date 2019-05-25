'use strict';
app.factory('ConfigurationService', ['BaseService','CommonService', function (baseService, commonService) {

    var serviceFactory = Object.create(baseService);
    serviceFactory.init('configuration');

    var _uploadConfiguration = async function (configurationFile) {
        //var container = $(this).parents('.model-configuration').first().find('.custom-file').first();
        if (configurationFile.file !== undefined && configurationFile.file !== null) {
            // Create FormData object
            var files = new FormData();

            // Looping over all files and add it to FormData object
            files.append(configurationFile.file.name, configurationFile.file);

            // Adding one more key to FormData object
            files.append('fileFolder', configurationFile.folder); files.append('title', configurationFile.title);
            files.append('description', configurationFile.description);

            var req = {
                url: this.prefixUrl + '/upload',
                type: "POST",
                headers: {
                },
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: files,
            };

            return await commonService.getApiResult(req)
        }
    };
    serviceFactory.uploadConfiguration = _uploadConfiguration;
    return serviceFactory;

}]);
