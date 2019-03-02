
app.component('productGeneral', {
    templateUrl: '/app/app-portal/pages/product/components/general/productGeneral.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', function ($rootScope, $scope, ngAppSettings) {
        var ctrl = this;
        //ctrl.tags = ctrl.product.tags;
        ctrl.dataTypes = $rootScope.globalSettings.dataTypes;
        ctrl.addProperty = function (type) {
            var i = $(".property").length;
            ctrl.product.properties.push({
                priority: 0,
                name: '',
                value: null,
                dataType: 0
            });
        };
        ctrl.initEditor = $rootScope.initEditor;
        ctrl.loadTags = function (query) {
            return [];
        };
    }],
    bindings: {
        product: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});