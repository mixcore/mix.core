app.config(function ($routeProvider, $locationProvider, $sceProvider, CommonServiceProvider) {
    $locationProvider.html5Mode(true);
    $routeProviderReference = $routeProvider;
    $routeProvider.otherwise({ redirectTo: "/portal" });
});
