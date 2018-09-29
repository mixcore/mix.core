
app.component('productMedias', {
    templateUrl: '/app-portal/pages/product/components/medias/productMedias.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
        ctrl.medias = $rootScope.medias;
        ctrl.init = function () {
            if ($rootScope.medias) {
                angular.forEach(ctrl.medias, function (value, key) {
                    value.isActived = ctrl.checkActived(value) !== undefined;
                });
                
            }
        };
        ctrl.checkActived = function (item) {
            if (ctrl.product.mediaNavs) {
                return ctrl.product.mediaNavs.find(function (nav) {
                    return nav.mediaId === item.id;
                });
            }
        };
        ctrl.activeMedia = function (media) {
            var currentItem = null;
            if (ctrl.product.mediaNavs === null) {
                ctrl.product.mediaNavs = [];
            }
            $.each(ctrl.product.mediaNavs, function (i, e) {
                if (e.mediaId === media.id) {
                    e.isActived = media.isActived;
                    currentItem = e;
                    return false;
                }
            });
            if (currentItem === null) {
                currentItem = {
                    description: media.description !== 'undefined' ? media.description : '',
                    image: media.fullPath,
                    mediaId: media.id,
                    product: ctrl.product.id,
                    specificulture: media.specificulture,
                    position: 0,
                    priority: ctrl.product.mediaNavs.length + 1,
                    isActived: true
                };
                media.isHidden = true;
                ctrl.product.mediaNavs.push(currentItem);
            }
        };
    }],
    bindings: {
        product: '=',
        medias: '=',
        loadMedia: '&',
        onDelete: '&',
        onUpdate: '&'
    }
});