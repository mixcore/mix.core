
app.component('attributeSetEdm', {
    templateUrl: '/app/app-portal/pages/attribute-set/components/edm/view.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
        ctrl.settings = $rootScope.globalSettings;        
    }],
    bindings: {
        model: '=',
    }
});