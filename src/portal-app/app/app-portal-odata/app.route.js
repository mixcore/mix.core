app.config(function ($routeProvider, $locationProvider, $sceProvider) {
    $locationProvider.html5Mode(true);
    $routeProvider.when("/portal/menu/list", {
        controller: "PositionController",
        templateUrl: "/app/app-portal-odata/pages/menu/list.html"
    });
    $routeProvider.when("/portal/menu/details/:id", {
        controller: "PositionController",
        templateUrl: "/app/app-portal-odata/pages/menu/details.html"
    });
    $routeProvider.when("/portal/menu/create", {
        controller: "PositionController",
        templateUrl: "/app/app-portal-odata/pages/menu/details.html"
    });
    $routeProvider.otherwise({ redirectTo: "/portal" });
});
