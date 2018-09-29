
app.component('productSeo', {
    templateUrl: '/app-portal/pages/product/components/seo/productSeo.html',
    bindings: {
        product: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});