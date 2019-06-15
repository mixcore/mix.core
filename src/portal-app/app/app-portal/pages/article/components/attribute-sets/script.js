
app.component('articleAttributeSets', {
    templateUrl: '/app/app-portal/pages/article/components/attribute-sets/view.html',
    controller: ['$rootScope', 'ArticleAttributeValueService', function ($rootScope, valueService) {
        var ctrl = this;
        ctrl.dataTypes = $rootScope.globalSettings.dataTypes;
        
    }],
    bindings: {
        article: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});