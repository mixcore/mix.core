app.config(function ($routeProvider, $locationProvider, $sceProvider) {
    $locationProvider.html5Mode(true);

    $routeProvider.when("/portal", {
        controller: "DashboardController",
        templateUrl: "/app/app-portal/pages/dashboard/dashboard.html"
    });


    $routeProvider.when("/portal/customer/details/:id", {
        controller: "CustomerController",
        templateUrl: "/app/app-portal/pages/customer/details.html"
    });

    $routeProvider.when("/portal/customer/list", {
        controller: "CustomerController",
        templateUrl: "/app/app-portal/pages/customer/list.html"
    });

    $routeProvider.when("/portal/product/list", {
        controller: "ProductController",
        templateUrl: "/app/app-portal/pages/product/list.html"
    });

    $routeProvider.when("/portal/product/details/:id", {
        controller: "ProductController",
        templateUrl: "/app/app-portal/pages/product/details.html"
    });

    $routeProvider.when("/portal/product/create", {
        controller: "ProductController",
        templateUrl: "/app/app-portal/pages/product/details.html"
    });
    $routeProvider.when("/portal/order/list", {
        controller: "OrderController",
        templateUrl: "/app/app-portal/pages/order/list.html"
    });

    $routeProvider.when("/portal/order/details/:id", {
        controller: "OrderController",
        templateUrl: "/app/app-portal/pages/order/details.html"
    });

    $routeProvider.when("/portal/order/create", {
        controller: "OrderController",
        templateUrl: "/app/app-portal/pages/order/details.html"
    });
    $routeProvider.when("/portal/article/list", {
        controller: "ArticleController",
        templateUrl: "/app/app-portal/pages/article/list.html"
    });

    $routeProvider.when("/portal/article/details/:id", {
        controller: "ArticleController",
        templateUrl: "/app/app-portal/pages/article/details.html"
    });

    $routeProvider.when("/portal/article/create", {
        controller: "ArticleController",
        templateUrl: "/app/app-portal/pages/article/details.html"
    });

    $routeProvider.when("/portal/page/list", {
        controller: "PageController",
        templateUrl: "/app/app-portal/pages/page/list.html"
    });

    $routeProvider.when("/portal/page/details/:id", {
        controller: "PageController",
        templateUrl: "/app/app-portal/pages/page/details.html"
    });

    $routeProvider.when("/portal/permission/list", {
        controller: "PermissionController",
        templateUrl: "/app/app-portal/pages/permission/list.html"
    });

    $routeProvider.when("/portal/permission/create", {
        controller: "PermissionController",
        templateUrl: "/app/app-portal/pages/permission/details.html"
    });

    $routeProvider.when("/portal/permission/details/:id", {
        controller: "PermissionController",
        templateUrl: "/app/app-portal/pages/permission/details.html"
    });

    $routeProvider.when("/portal/page/data/:id", {
        controller: "PageController",
        templateUrl: "/app/app-portal/pages/page/data.html"
    });

    $routeProvider.when("/portal/page/create", {
        controller: "PageController",
        templateUrl: "/app/app-portal/pages/page/details.html"
    });

    $routeProvider.when("/portal/module/list", {
        controller: "ModuleController",
        templateUrl: "/app/app-portal/pages/module/list.html"
    });

    $routeProvider.when("/portal/module/data/:id", {
        controller: "ModuleController",
        templateUrl: "/app/app-portal/pages/module/data.html"
    });

    $routeProvider.when("/portal/module-data/details/:moduleId/:id", {
        controller: "ModuleDataController",
        templateUrl: "/app/app-portal/pages/moduleData/details.html"
    });

    $routeProvider.when("/portal/module-data/details/:moduleId", {
        controller: "ModuleDataController",
        templateUrl: "/app/app-portal/pages/moduleData/details.html"
    });

    $routeProvider.when("/portal/module/details/:id", {
        controller: "ModuleController",
        templateUrl: "/app/app-portal/pages/module/details.html"
    });

    $routeProvider.when("/portal/module/create", {
        controller: "ModuleController",
        templateUrl: "/app/app-portal/pages/module/details.html"
    });

    $routeProvider.when("/portal/media/list", {
        controller: "MediaController",
        templateUrl: "/app/app-portal/pages/media/list.html"
    });

    $routeProvider.when("/portal/media/details/:id", {
        controller: "MediaController",
        templateUrl: "/app/app-portal/pages/media/details.html"
    });

    $routeProvider.when("/portal/media/create", {
        controller: "MediaController",
        templateUrl: "/app/app-portal/pages/media/details.html"
    });


    $routeProvider.when("/portal/language/list", {
        controller: "LanguageController",
        templateUrl: "/app/app-portal/pages/language/list.html"
    });

    $routeProvider.when("/portal/language/details/:id", {
        controller: "LanguageController",
        templateUrl: "/app/app-portal/pages/language/details.html"
    });

    $routeProvider.when("/portal/language/create", {
        controller: "LanguageController",
        templateUrl: "/app/app-portal/pages/language/details.html"
    });

    $routeProvider.when("/portal/configuration/list", {
        controller: "ConfigurationController",
        templateUrl: "/app/app-portal/pages/configuration/list.html"
    });

    $routeProvider.when("/portal/configuration/details/:id", {
        controller: "ConfigurationController",
        templateUrl: "/app/app-portal/pages/configuration/details.html"
    });

    $routeProvider.when("/portal/configuration/create", {
        controller: "ConfigurationController",
        templateUrl: "/app/app-portal/pages/configuration/details.html"
    });

    $routeProvider.when("/portal/file/list", {
        controller: "FileController",
        templateUrl: "/app/app-portal/pages/file/list.html"
    });

    $routeProvider.when("/portal/file/details", {
        controller: "FileController",
        templateUrl: "/app/app-portal/pages/file/details.html"
    });

    $routeProvider.when("/portal/file/create", {
        controller: "FileController",
        templateUrl: "/app/app-portal/pages/file/details.html"
    });

    $routeProvider.when("/portal/theme/list", {
        controller: "ThemeController",
        templateUrl: "/app/app-portal/pages/theme/list.html"
    });

    $routeProvider.when("/portal/theme/details/:id", {
        controller: "ThemeController",
        templateUrl: "/app/app-portal/pages/theme/details.html"
    });

    $routeProvider.when("/portal/theme/create", {
        controller: "ThemeController",
        templateUrl: "/app/app-portal/pages/theme/details.html"
    });

    $routeProvider.when("/portal/template/list/:themeId/:folderType", {
        controller: "TemplateController",
        templateUrl: "/app/app-portal/pages/template/list.html"
    });

    $routeProvider.when("/portal/template/list/:themeId", {
        controller: "TemplateController",
        templateUrl: "/app/app-portal/pages/template/list.html"
    });

    $routeProvider.when("/portal/template/details/:themeId/:folderType/:id", {
        controller: "TemplateController",
        templateUrl: "/app/app-portal/pages/template/details.html"
    });

    $routeProvider.when("/portal/template/create/:themeId/:folderType", {
        controller: "TemplateController",
        templateUrl: "/app/app-portal/pages/template/details.html"
    });

    $routeProvider.when("/portal/role/list", {
        controller: "RoleController",
        templateUrl: "/app/app-portal/pages/role/list.html"
    });

    $routeProvider.when("/portal/role/details/:id", {
        controller: "RoleController",
        templateUrl: "/app/app-portal/pages/role/details.html"
    });

    $routeProvider.when("/portal/user/list", {
        controller: "UserController",
        templateUrl: "/app/app-portal/pages/user/list.html"
    });

    $routeProvider.when("/portal/user/details/:id", {
        controller: "UserController",
        templateUrl: "/app/app-portal/pages/user/details.html"
    });

    $routeProvider.when("/portal/user/create", {
        controller: "UserController",
        templateUrl: "/app/app-portal/pages/user/register.html"
    });

    $routeProvider.when("/portal/app-settings/details", {
        controller: "AppSettingsController",
        templateUrl: "/app/app-portal/pages/app-settings/details.html"
    });
    
    $routeProvider.when("/portal/import", {
        controller: "ImportFileController",
        templateUrl: "/app/app-portal/pages/import/details.html"
    });

    $routeProvider.when("/portal/culture/list", {
        controller: "CultureController",
        templateUrl: "/app/app-portal/pages/culture/list.html"
    });
    $routeProvider.when("/portal/culture/details/:id", {
        controller: "CultureController",
        templateUrl: "/app/app-portal/pages/culture/details.html"
    });
    $routeProvider.when("/portal/culture/create", {
        controller: "CultureController",
        templateUrl: "/app/app-portal/pages/culture/details.html"
    });

    $routeProvider.otherwise({ redirectTo: "/portal" });
});
