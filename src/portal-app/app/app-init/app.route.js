app.config(function ($routeProvider, $locationProvider, $sceProvider) {
    $locationProvider.html5Mode(true);

    $routeProvider.when("/init", {
        controller: "Step1Controller",
        templateUrl: "/app/app-init/pages/step1/index.html"
    });

    $routeProvider.when("/init/step2", {
        controller: "Step2Controller",
        templateUrl: "/app/app-init/pages/step2/view.html"
    });
    $routeProvider.when("/init/step3", {
        controller: "Step3Controller",
        templateUrl: "/app/app-init/pages/step3/view.html"
    });
    $routeProvider.when("/init/step4", {
        controller: "Step4Controller",
        templateUrl: "/app/app-init/pages/step4/view.html"
    });
    $routeProvider.when("/init/step5", {
        controller: "Step5Controller",
        templateUrl: "/app/app-init/pages/step5/view.html"
    });
    $routeProvider.otherwise({ redirectTo: "/init" });
});
