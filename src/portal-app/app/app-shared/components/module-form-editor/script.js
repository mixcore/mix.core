
modules.component('moduleFormEditor', {
    templateUrl: '/app/app-shared/components/module-form-editor/view.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', function ($rootScope, $scope, ngAppSettings) {
        var ctrl = this;
        ctrl.icons = ngAppSettings.icons;
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
       }
    ],
    bindings: {
        data: '=',
        inputClass: '=',
        isShowTitle: '=',
        fieldTitle: '='
    }
});