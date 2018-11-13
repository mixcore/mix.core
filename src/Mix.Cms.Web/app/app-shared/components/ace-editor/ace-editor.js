
modules.component('aceEditor', {
    templateUrl: '/app/app-shared/components/ace-editor/ace-editor.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings',
        function ($rootScope, $scope, ngAppSettings) {
            var ctrl = this;
            ctrl.editor;
            ctrl.id = Math.floor(Math.random() * 100) + 1;
            
            ctrl.initAce = function () {
                setTimeout(() => {
                    ctrl.updateEditors();
                    $scope.$apply();
                }, 200);

            };
            ctrl.updateEditors = function () {
                $.each($('#code-editor-' + ctrl.id), function (i, e) {
                    var container = $(this);
                    var editor = ace.edit(e);
                    switch (ctrl.ext) {
                        case '.json':
                            editor.session.setMode("ace/mode/json");
                            break;
                        case '.js':
                            editor.session.setMode("ace/mode/javascript");
                            break;
                        case '.css':
                            editor.session.setMode("ace/mode/css");
                            break;
                        case '.cshtml':
                            editor.session.setMode("ace/mode/razor");
                            break;
                        case '.cs':
                            editor.session.setMode("ace/mode/csharp");
                            break;
                        default:
                            editor.session.setMode("ace/mode/razor");
                            break;
                    }
                    editor.setTheme("ace/theme/chrome");
                    //editor.setReadOnly(true);
                    editor.$blockScrolling = Infinity;
                    editor.session.setUseWrapMode(true);
                    editor.setOptions({
                        maxLines: 50,
                        fontSize: 11
                    });
                    editor.getSession().on('change', function (e) {
                        // e.type, etc
                        ctrl.content = editor.getValue();
                    });
                    editor.getSession().on('paste', function (e) {
                        // e.type, etc
                        ctrl.content = editor.getValue();
                    });
                    ctrl.editor = editor;
                });
            };
        }],
    bindings: {
        content: '=',
        ext: '='
    }
});