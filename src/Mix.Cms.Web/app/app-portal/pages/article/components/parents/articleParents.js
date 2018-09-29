
app.component('articleParents', {
    templateUrl: '/app-portal/pages/article/components/parents/articleParents.html',
    bindings: {
        article: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});