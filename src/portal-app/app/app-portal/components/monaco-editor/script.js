modules.component('monacoEditor', {
    templateUrl: '/app/app-portal/components/monaco-editor/view.html',
    controller: ['$rootScope', '$scope', '$element',
        function ($rootScope, $scope, $element) {
            var ctrl = this;
            ctrl.previousId = null;
            ctrl.editor = null;
            ctrl.isVisible = false;
            ctrl.id = Math.floor(Math.random() * 100) + 1;
            ctrl.$onChanges = (changes) => {
                if (changes.content) {
                    ctrl.updateContent(changes.content);
                }
                if (changes.isVisible) {
                    ctrl.updateEditors();
                }
            };

            this.$doCheck = function () {
                if (ctrl.previousId != null && ctrl.previousId !== ctrl.contentId) {
                    ctrl.previousId = ctrl.contentId;
                    ctrl.updateContent(ctrl.content);
                }
                if (ctrl.isVisible && ctrl.editor) {
                    setTimeout(() => {
                        var h = ctrl.editor.getModel().getLineCount() * 18;
                        $($element).height(h);
                        ctrl.editor.layout();
                    }, 200);

                }
            }.bind(this);
            ctrl.initEditor = function () {

                setTimeout(() => {
                    ctrl.previousId = ctrl.contentId;
                    ctrl.updateEditors();
                    $scope.$apply();
                }, 200);

            };
            ctrl.updateContent = function (content) {
                ctrl.editor.setValue(content);
                var h = ctrl.editor.getModel().getLineCount() * 18;
                $($element).height(h);
                ctrl.editor.layout();
            };
            ctrl.updateEditors = function () {
                $.each($($element).find('.code-editor'), function (i, e) {
                    //var container = $(this);
                    if (e) {
                        var model = {
                            value: ctrl.content || ctrl.defaultContent,
                            contextmenu: false
                        };
                        switch (ctrl.ext) {
                            case '.json':
                                model.language = 'json';
                                break;
                            case '.js':
                                model.language = 'javascript';

                                break;
                            case '.css':
                                model.language = 'css';
                                break;
                            case '.cshtml':
                                model.language = 'razor';
                                break;
                            case '.cs':
                                model.language = 'csharp';
                                break;
                            default:
                                model.language = 'razor';
                                break;
                        }
                        ctrl.editor = monaco.editor.create(e, model);

                        ctrl.editor.model.onDidChangeContent(() => {
                            ctrl.content = ctrl.editor.model.getValue();
                        });
                        ctrl.editor.addCommand(monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_S, function () {
                            var btn = document.getElementById('btnToSubmit');
                            btn.click();
                        });
                        setTimeout(() => {
                            var h = ctrl.editor.getModel().getLineCount() * 18;
                            $(e).height(h);
                            ctrl.editor.layout();
                        }, 200);

                    }
                });
            };
        }
    ],
    bindings: {
        content: '=',
        defaultContent: '=?',
        contentId: '=',
        isVisible: '=',
        ext: '='
    }
});