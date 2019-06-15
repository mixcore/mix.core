
app.component('articleAttributeSets', {
    templateUrl: '/app/app-portal/pages/article/components/attribute-sets/view.html',
    controller: ['$rootScope', 'ngAppSettings', function ($rootScope, ngAppSettings) {
        var ctrl = this;
        ctrl.dataTypes = $rootScope.globalSettings.dataTypes;
        
        ctrl.selectPane = function (pane) {
            console.log(pane);
        };        
    }],
    bindings: {
        article: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});