modules.component('monacoEditor', {
    templateUrl: '/app/app-portal/components/monaco-editor/view.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings',
        function ($rootScope, $scope, ngAppSettings) {
            var ctrl = this;
            ctrl.previousId = null;
            ctrl.editor = null;
            ctrl.id = Math.floor(Math.random() * 100) + 1;
            ctrl.$onChanges = (changes) => {
                if (changes.content) {
                    ctrl.updateEditors();
                }
            };

            this.$doCheck = function () {
                if (ctrl.previousId != null && ctrl.previousId !== ctrl.contentId) {
                    ctrl.previousId = ctrl.contentId;
                    ctrl.updateEditors();
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
            };
            ctrl.updateEditors = function () {
                $.each($('#code-editor-' + ctrl.id), function (i, e) {
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
                        
                        ctrl.editor.model.onDidChangeContent(()=>{
                            ctrl.content = ctrl.editor.model.getValue();
                        });
                        ctrl.editor.addCommand(monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_S, function() {
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
        ext: '='
    }
});