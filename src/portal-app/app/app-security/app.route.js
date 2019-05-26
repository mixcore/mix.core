app.config(function ($routeProvider, $locationProvider, $sceProvider) {
    $locationProvider.html5Mode(true);

    $routeProvider.when("/security/login", {
        controller: "LoginController",
        templateUrl: "/app/app-security/pages/login/view.html"
    });


    $routeProvider.when("/security/register", {
        controller: "RegisterController",
        templateUrl: "/app/app-security/pages/register/view.html"
    });
    $routeProvider.when("/security/forgot-password", {
        controller: "ForgotPasswordController",
        templateUrl: "/app/app-security/pages/forgot-password/view.html"
    });
    $routeProvider.when("/security/reset-password", {
        controller: "ResetPasswordController",
        templateUrl: "/app/app-security/pages/reset-password/view.html"
    });

    $routeProvider.otherwise({ redirectTo: "/security/login" });
});
