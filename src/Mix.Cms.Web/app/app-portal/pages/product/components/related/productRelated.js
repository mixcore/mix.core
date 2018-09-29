
app.component('productRelated', {
    templateUrl: '/app-portal/pages/product/components/related/productRelated.html',
    controller: function () {
        var ctrl = this;
        ctrl.activeProduct = function (pr) {
            var currentItem = null;
            $.each(ctrl.product.productNavs, function (i, e) {
                if (e.relatedProductId == pr.id) {
                    e.isActived = pr.isActived;
                    currentItem = e;
                    return false;
                }
            });
            if (currentItem == null) {
                currentItem = {
                    relatedProductId: pr.id,
                    sourceProductId: ctrl.product.id,
                    specificulture: ctrl.product.specificulture,
                    priority: ctrl.product.productNavs.length + 1,
                    relatedProduct: pr,
                    isActived: true
                };
                pr.isHidden = true;
                ctrl.product.productNavs.push(currentItem);
            }
        }
    },
    bindings: {
        product: '=',
        list: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});