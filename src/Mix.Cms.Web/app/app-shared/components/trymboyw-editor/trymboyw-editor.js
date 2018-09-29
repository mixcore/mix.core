
modules.component('trymboywEditor', {
    templateUrl: '/app-shared/components/trymboyw-editor/trymboyw-editor.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings',
        function ($rootScope, $scope, ngAppSettings) {
            var ctrl = this;
            ctrl.id = Math.random().toString(36).substring(7);
            ctrl.init = function (e) {
                setTimeout(function () {
                    ctrl.textArea = $(document.getElementById(ctrl.id));
                    ctrl.textArea.trumbowyg(ngAppSettings.editorConfigurations.plugins)
                    .on('tbwchange ', function () { ctrl.content = ctrl.textArea.val() })    
                    .trumbowyg('html', ctrl.content);
                }, 500);
            }
            
            ctrl.translate = function (keyword) {
                return $rootScope.translate(keyword);
            };
        }],
    bindings: {
        content: '='
    }
});