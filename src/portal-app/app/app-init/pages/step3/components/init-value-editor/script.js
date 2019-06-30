
modules.component('initValueEditor', {
    templateUrl: '/app/app-init/pages/step2/components/init-value-editor/view.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings','FileService'
        , function ($rootScope, $scope, ngAppSettings, fileService) {
        var ctrl = this;
        ctrl.icons = ngAppSettings.icons;
        ctrl.mediaFile = {
            file: null,
            fullPath: '',
            fileFolder: 'content/site',
            title: '',
            description: ''
        };
        this.dataTypes = ngAppSettings.dataTypes;
        ctrl.initEditor = function () {
            ctrl.data.value = ctrl.data.default || null;
            setTimeout(function () {
                // Init Code editor
                $.each($('.code-editor'), function (i, e) {
                    var container = $(this);
                    var editor = ace.edit(e);
                    if (container.hasClass('json')) {
                        editor.session.setMode("ace/mode/json");
                    }
                    else {
                        editor.session.setMode("ace/mode/razor");
                    }
                    editor.setTheme("ace/theme/chrome");
                    //editor.setReadOnly(true);

                    editor.session.setUseWrapMode(true);
                    editor.setOptions({
                        maxLines: Infinity
                    });
                    editor.getSession().on('change', function (e) {
                        // e.type, etc
                        $(container).parent().find('.code-content').val(editor.getValue());
                    });
                })
                $.each($('.editor-content'), function (i, e) {
                    var $demoTextarea = $(e);
                    $demoTextarea.trumbowyg({
                        semantic: false
                    }).on('tbwblur', function () {
                        ctrl.data.value = $demoTextarea.val();
                    });
                });
            }, 200);
        };
        ctrl.selectFile = function (file, errFiles) {
            if (file !== undefined && file !== null) {
                ctrl.mediaFile.folder = ctrl.folder ? ctrl.folder : 'Media';
                ctrl.mediaFile.title = ctrl.title ? ctrl.title : '';
                ctrl.mediaFile.description = ctrl.description ? ctrl.description : '';
                ctrl.mediaFile.file = file;

                ctrl.uploadFile(file);
            }
        };
        ctrl.uploadFile = async function (file) {
            if (file !== null) {
                $rootScope.isBusy = true;
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = async function () {
                    ctrl.mediaFile.fileName = file.name.substring(0, file.name.lastIndexOf('.'));
                    ctrl.mediaFile.extension = file.name.substring(file.name.lastIndexOf('.'));
                    ctrl.mediaFile.fileStream = reader.result;
                    var resp = await fileService.save(ctrl.mediaFile);
                    if (resp && resp.isSucceed) {
                        ctrl.data.value = resp.data.webPath;
                        ctrl.srcUrl = resp.data.webPath;
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                    else {
                        if (resp) { $rootScope.showErrors(resp.errors); }
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                };
                reader.onerror = function (error) {

                };
            }
            else {
                return null;
            }

        }

    }
    ],
    bindings: {
        data: '=',
        inputClass: '=',
        isShowTitle: '=',
        title: '='
    }
});