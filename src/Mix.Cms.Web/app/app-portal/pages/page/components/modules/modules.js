
app.component('pageModules', {
    templateUrl: '/app-portal/pages/page/components/modules/modules.html',
    bindings: {
        page: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});