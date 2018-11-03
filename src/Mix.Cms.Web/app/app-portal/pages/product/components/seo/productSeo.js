
app.component('productSeo', {
    templateUrl: '/app/app-portal/pages/product/components/seo/productSeo.html',
    bindings: {
        product: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});