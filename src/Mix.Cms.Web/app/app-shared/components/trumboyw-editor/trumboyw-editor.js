
modules.component('trumboywEditor', {
    templateUrl: '/app/app-shared/components/trumboyw-editor/trumboyw-editor.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings',
        function ($rootScope, $scope, ngAppSettings) {
            var ctrl = this;
            ctrl.id = Math.random().toString(36).substring(7);
            ctrl.init = function (e) {
                $.trumbowyg.svgPath = '/assets/icons.svg';
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