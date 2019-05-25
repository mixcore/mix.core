
app.component('productModules', {
    templateUrl: '/app/app-portal/pages/product/components/modules/productModules.html',
    bindings: {
        product: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});