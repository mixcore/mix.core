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
                            value: ctrl.content,                            
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
                        
                        // editor.setTheme("ace/theme/chrome");
                        // //editor.setReadOnly(true);
                        // if (ctrl.content) {
                        //     editor.setValue(ctrl.content);
                        // }
                        // editor.$blockScrolling = Infinity;
                        // editor.session.setUseWrapMode(true);
                        // editor.setOptions({
                        //     enableBasicAutocompletion: true,
                        //     enableSnippets: true,
                        //     enableLiveAutocompletion: false,
                        //     maxLines: 50,
                        //     fontSize: 11
                        // });
                        // editor.getSession().on('change', function (e) {
                        //     // e.type, etc
                        //     ctrl.content = editor.getValue();
                        // });
                        // editor.getSession().on('paste', function (e) {
                        //     // e.type, etc
                        //     ctrl.content = editor.getValue();
                        // });
                        // editor.commands.addCommand({
                        //     name: 'saveFile',
                        //     bindKey: {
                        //     win: 'Ctrl-S',
                        //     mac: 'Command-S',
                        //     sender: 'editor|cli'
                        //     },
                        //     exec: function(env, args, request) {
                                
                        //        var btn = document.getElementById('btnToSubmit');
                        //        btn.click();
                        //     }
                        //     });
                        setTimeout(() => {
                            var h = ctrl.editor.getModel().getLineCount() * 8;
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
        contentId: '=',
        ext: '='
    }
});