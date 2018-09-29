
app.component('articleSeo', {
    templateUrl: '/app-portal/pages/article/components/seo/articleSeo.html',
    bindings: {
        article: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});