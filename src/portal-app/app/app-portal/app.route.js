app.config(function ($routeProvider, $locationProvider, $sceProvider, ngAppSettings) {
    $locationProvider.html5Mode(true);
    var data = $.parseJSON($('#portal-menus').val());
    ngAppSettings.routes = data.routes;
    angular.forEach(ngAppSettings.routes, function(cate, key) {
        if(cate.items.length){            
            angular.forEach(cate.items, function(item, key) {
                $routeProvider.when(item.path, {
                    controller: item.controller,
                    templateUrl: item.templatePath
                });
            });
        }        
    });
    $routeProvider.otherwise({ redirectTo: "/portal" });
});
