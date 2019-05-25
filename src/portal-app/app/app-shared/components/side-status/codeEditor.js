
modules.component('codeEditor', {
    templateUrl: 'codeEditor.html',
    bindings: {
        product: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});