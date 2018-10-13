modules.component('aceEditor', {
    aceFileUrl: '/app-shared/components/ace-editor/aceEditor.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'CommonService', function ($rootScope, $scope, ngAppSettings, commonService) {
        var vm = this;
        vm.id = Math.random();
        vm.initAce = function () {
                vm.updateEditors();
                $scope.$apply();            
        };
        vm.updateEditors = function () {
            var container = $('#' + id);
            var editor = ace.edit(container);
            var val = $(this).next('input').val();
            editor.setValue(val);
            if (container.hasClass('json')) {
                editor.session.setMode("ace/mode/json");
            }
            else {
                editor.session.setMode("ace/mode/razor");
            }
            editor.setTheme("ace/theme/chrome");

            editor.session.setUseWrapMode(true);
            editor.setOptions({
                maxLines: Infinity
            });
            editor.getSession().on('change', function (e) {
                // e.type, etc
                $(container).parent().find('.code-content').val(editor.getValue());
            });
        };
    }],
    bindings: {
        content: '=',
    }
});