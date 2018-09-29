app.config(function ($routeProvider, $locationProvider, $sceProvider) {
    $locationProvider.html5Mode(true);

    $routeProvider.when("/init/login", {
        controller: "loginController",
        templateUrl: "/app-init/pages/login/login.html"
    });

    $routeProvider.when("/init", {
        controller: "Step1Controller",
        templateUrl: "/app-init/pages/step1/index.html"
    });

    $routeProvider.when("/init/step2", {
        controller: "Step2Controller",
        templateUrl: "/app-init/pages/step2/index.html"
    });

    $routeProvider.otherwise({ redirectTo: "/init" });
});
