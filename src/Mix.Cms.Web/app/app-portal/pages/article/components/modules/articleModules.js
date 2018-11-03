
app.component('articleModules', {
    templateUrl: '/app/app-portal/pages/article/components/modules/articleModules.html',
    bindings: {
        article: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});