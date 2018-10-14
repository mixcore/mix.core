'use strict';
app.factory('LanguageService', ['BaseService','CommonService', function (baseService, commonService) {

    var serviceFactory = Object.create(baseService);
    serviceFactory.init('language');

    var _uploadLanguage = async function (languageFile) {
        //var container = $(this).parents('.model-language').first().find('.custom-file').first();
        if (languageFile.file !== undefined && languageFile.file !== null) {
            // Create FormData object
            var files = new FormData();

            // Looping over all files and add it to FormData object
            files.append(languageFile.file.name, languageFile.file);

            // Adding one more key to FormData object
            files.append('fileFolder', languageFile.folder); files.append('title', languageFile.title);
            files.append('description', languageFile.description);

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
    serviceFactory.uploadLanguage = _uploadLanguage;
    return serviceFactory;

}]);
