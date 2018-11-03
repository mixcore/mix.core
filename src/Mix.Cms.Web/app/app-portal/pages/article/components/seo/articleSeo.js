
app.component('articleSeo', {
    templateUrl: '/app/app-portal/pages/article/components/seo/articleSeo.html',
    bindings: {
        article: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});