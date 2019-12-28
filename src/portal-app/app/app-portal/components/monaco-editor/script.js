modules.component('monacoEditor', {
    templateUrl: '/app/app-portal/components/monaco-editor/view.html',
    bindings: {
        editor: '=?',
        content: '=',
        defaultContent: '=?',
        contentId: '=',
        isVisible: '=',
        isReadonly: '=?',
        lineCount: '=?',
        ext: '='
    },
    controller: ['$rootScope', '$scope', '$element',
        function ($rootScope, $scope, $element) {
            var ctrl = this;
            ctrl.previousId = null;
            ctrl.minHeight = 500;
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
                ctrl.lineCount = parseInt(ctrl.lineCount) || 100;
                setTimeout(() => {
                    ctrl.previousId = ctrl.contentId;
                    ctrl.updateEditors();
                    $scope.$apply();
                }, 200);

            };
            ctrl.updateContent = function (content) {
                ctrl.editor.setValue(content);
                // lineCount = ctrl.editor.getModel().getLineCount();

                // var h = ctrl.editor.getModel().getLineCount() * 18;
                var h = ctrl.lineCount * 18;
                $($element).height(h);
                ctrl.editor.layout();
            };
            ctrl.updateEditors = function () {
                $.each($($element).find('.code-editor'), function (i, e) {
                    //var container = $(this);
                    if (e) {
                        var model = {
                            value: ctrl.content || ctrl.defaultContent,
                            readOnly: ctrl.isReadonly || false,
                            contextmenu: false,
                            // theme: "vs-dark",
                            formatOnType: true,
                            formatOnPaste: true,
                            wordWrap: 'on',
                            automaticLayout: true, // the important part
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
                            // var h = ctrl.editor.getModel().getLineCount() * 18;
                            // h = h < ctrl.minHeight ? ctrl.minHeight : h;
                            var h = ctrl.lineCount * 20;
                            $(e).height(h);
                            ctrl.editor.layout();
                        }, 200);

                    }
                });
            };
            ctrl.fullscreen = function (event) {
                // $.each($($element).find('.code-editor'), function (i, e) {
                //     //var container = $(this);
                //     if (e) {}
                // });
                // console.log($(event.target));

                $(event.target).parents('.monaco-editor').toggleClass('monaco-editor-full');

                // $('.monaco-editor').toggleClass('monaco-editor-full');
                var h;

                ctrl.editor.dispose();
                // setTimeout(() => {
                    if ($(".monaco-editor.container-code-editor.monaco-editor-full")[0]) {
                        // Do something if class exists
                        h = window.innerHeight;
                        $('.monaco-editor.container-code-editor.monaco-editor-full .code-editor').height(h);
                        document.body.style.overflow = 'hidden';
                    } else {
                        // Do something if class does not exist
                        h = ctrl.lineCount * 12;
                        $('.monaco-editor .code-editor').height(h);
                        document.body.style.overflow = 'visible';
                    }
                    ctrl.editor.layout();

                ctrl.updateEditors();
                // }, 200);

            };
        }
    ]
});