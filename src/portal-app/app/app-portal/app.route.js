app.config(function ($routeProvider, $locationProvider, $sceProvider, CommonServiceProvider) {
    $locationProvider.html5Mode(true);
    var commonService  =CommonServiceProvider.$get();
    commonService.loadJArrayData('portal-menus.json').then(resp=>{
        angular.forEach(resp.data, function(item, key) {
            if(item.type=='item'){
                $routeProvider.when(item.path, {
                    controller: item.controller,
                    templateUrl: item.templatePath
                });
                angular.forEach(item.subMenus.data, function(sub, key) {
                    $routeProvider.when(sub.path, {
                        controller: sub.controller,
                        templateUrl: sub.templatePath
                    });
                });
            }
            else{
                angular.forEach(item.data, function(sub, key) {
                    $routeProvider.when(sub.path, {
                        controller: sub.controller,
                        templateUrl: sub.templatePath
                    });
                    angular.forEach(sub.subMenus.data, function(sub1, key) {
                        $routeProvider.when(sub1.path, {
                            controller: sub1.controller,
                            templateUrl: sub1.templatePath
                        });
                    });
                });
            }
        });
    });
    
    $routeProvider.otherwise({ redirectTo: "/portal" });
});
