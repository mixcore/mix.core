
modules.component('aceEditor', {
    templateUrl: '/app-shared/components/ace-editor/ace-editor.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings',
        function ($rootScope, $scope, ngAppSettings) {
            var ctrl = this;
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
                    if (container.hasClass('json')) {
                        editor.session.setMode("ace/mode/json");
                    }
                    else {
                        editor.session.setMode("ace/mode/razor");
                    }
                    editor.setTheme("ace/theme/chrome");
                    //editor.setReadOnly(true);
                    editor.$blockScrolling = Infinity;
                    editor.session.setUseWrapMode(true);
                    editor.setOptions({
                        maxLines: Infinity
                    });
                    editor.getSession().on('change', function (e) {
                        // e.type, etc
                        ctrl.content = editor.getValue();
                    });
                });
            };
        }],
    bindings: {
        content: '='
    }
});