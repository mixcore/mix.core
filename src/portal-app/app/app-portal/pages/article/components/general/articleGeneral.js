
app.component('articleGeneral', {
    templateUrl: '/app/app-portal/pages/article/components/general/articleGeneral.html',
    controller: ['$rootScope', 'ngAppSettings', function ($rootScope, ngAppSettings) {
        var ctrl = this;
        ctrl.dataTypes = $rootScope.globalSettings.dataTypes;
        ctrl.$onInit = function(){
            ctrl.isAdmin = $rootScope.isAdmin;
        };
        ctrl.addProperty = function (type) {
            var i = $(".property").length;
            ctrl.article.properties.push({
                title: '',
                name: '',
                value: null,
                dataType: '7'
            });
        };        
    }],
    bindings: {
        article: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});