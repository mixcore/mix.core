
app.component('productParents', {
    templateUrl: '/app-portal/pages/product/components/parents/productParents.html',
    bindings: {
        product: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});