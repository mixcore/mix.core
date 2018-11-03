
modules.component('apiFile', {
    templateUrl: '/app/app-shared/components/api-file/api-file.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', function PortalTemplateController($rootScope, $scope) {
        var ctrl = this;
        ctrl.selectFile = function (file, errFiles) {
            if (file !== undefined && file !== null) {
                ctrl.getBase64(file);
            }
        };

        ctrl.uploadFile = function (file) {
            // Create FormData object
            var files = new FormData();
            var folder = ctrl.folder;
            var title = ctrl.title;
            var description = ctrl.description;
            // Looping over all files and add it to FormData object
            files.append(file.name, file);

            // Adding one more key to FormData object
            files.append('fileFolder', folder);
            files.append('title', title);
            files.append('description', description);
            $.ajax({
                url: '/' + SW.Common.currentLanguage + '/media/upload', //'/tts/UploadImage',
                type: "POST",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: files,
                success: function (result) {
                    ctrl.src = result.data.fullPath;
                    $scope.$apply();
                },
                error: function (err) {
                    return '';
                }
            });

        }
        ctrl.getBase64 = function (file) {
            if (file !== null) {
                $rootScope.isBusy = true;
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = function () {
                    var index = reader.result.indexOf(',') + 1;
                    var base64 = reader.result.substring(index);
                    ctrl.postedFile.fileName = file.name.substring(0, file.name.lastIndexOf('.'));
                    ctrl.postedFile.extension = file.name.substring(file.name.lastIndexOf('.'));
                    ctrl.postedFile.fileStream = reader.result;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                };
                reader.onerror = function (error) {

                };
            }
            else {
                return null;
            }
        }
    }],
    bindings: {
        header: '=',
        title: '=',
        postedFile: '=',
    }
});