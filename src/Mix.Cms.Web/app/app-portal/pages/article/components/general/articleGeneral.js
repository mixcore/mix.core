
app.component('articleGeneral', {
    templateUrl: '/app/app-portal/pages/article/components/general/articleGeneral.html',
    controller: ['ngAppSettings', function (ngAppSettings) {
        var ctrl = this;
        ctrl.dataTypes = ngAppSettings.dataTypes;
        
        ctrl.addProperty = function (type) {
            var i = $(".property").length;
            ctrl.article.properties.push({
                priority: 0,
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