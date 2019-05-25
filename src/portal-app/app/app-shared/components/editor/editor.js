
modules.component('editor', {
    templateUrl: 'editor.html',
    controller: function ($scope) {

    },
    bindings: {
        dataType: '=',
        value: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});