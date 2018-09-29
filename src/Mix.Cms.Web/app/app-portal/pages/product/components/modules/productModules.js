
app.component('productModules', {
    templateUrl: '/app-portal/pages/product/components/modules/productModules.html',
    bindings: {
        product: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});