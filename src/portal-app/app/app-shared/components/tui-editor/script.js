modules.component('tuiEditor', {
    templateUrl: '/app/app-shared/components/tui-editor/view.html',
    controller: ['$rootScope', '$scope','$element', 'ngAppSettings',
        function ($rootScope, $scope, $element, ngAppSettings) {
            var ctrl = this;
            ctrl.previousId = null;
            ctrl.editor = null;
            ctrl.init = function () {
                ctrl.guid = $rootScope.generateUUID();
                setTimeout(() => {
                    ctrl.editor =new tui.Editor({
                        el: document.querySelector('#tui-' + ctrl.guid),
                        initialEditType: 'markdown',
                        previewStyle: 'vertical',
                        height: '300px',
                        initialValue: ctrl.content,
                        events: {
                            // change: ctrl.updateContent,
                            // focus: () => onFocus(),
                            blur: () => ctrl.updateContent(),
                          },
                      });
                    ctrl.toolbar = ctrl.editor.getUI().getToolbar();
                    ctrl.toolbar.addButton({
                    name: 'fullscreen',
                    tooltip: 'fullscreen',
                    $el: $('<button onclick="fsClick()" class="mi mi-FullScreen mi-lg mi-fw text-secondary" type="button"></button>')
                    }, 1);
                                         
                }, 100);      
            };
            window.fsClick=function(){
                $(".tui-editor-defaultUI").toggleClass("fs");
            }; 
            ctrl.updateContent = function () {
                ctrl.content = ctrl.editor.getMarkdown();
            };
            
        }
    ],
    bindings: {
        content: '='
    }
});