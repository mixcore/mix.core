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
    $routeProvider.when("/portal/post/list", {
        controller: "PostController",
        templateUrl: "/app/app-portal/pages/post/list.html"
    });

    $routeProvider.when("/portal/post/details/:id", {
        controller: "PostController",
        templateUrl: "/app/app-portal/pages/post/details.html"
    });
   
    $routeProvider.when("/portal/post/gallery-details/:id", {
        controller: "PostController",
        templateUrl: "/app/app-portal/pages/post/gallery-details.html"
    });
    $routeProvider.when("/portal/post/create-gallery", {
        controller: "PostController",
        templateUrl: "/app/app-portal/pages/post/gallery-details.html"
    });
    $routeProvider.when("/portal/post/create", {
        controller: "PostController",
        templateUrl: "/app/app-portal/pages/post/details.html"
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

    $routeProvider.when("/portal/page/page-post/list/:id", {
        controller: "PagePostController",
        templateUrl: "/app/app-portal/pages/page-post/list.html"
    });
   
    $routeProvider.when("/portal/page/page-gallery/list/:id", {
        controller: "PageGalleryController",
        templateUrl: "/app/app-portal/pages/page-gallery/list.html"
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

    $routeProvider.when("/portal/module-post/list/:id", {
        controller: "ModulePostController",
        templateUrl: "/app/app-portal/pages/module-post/list.html"
    });
   
    $routeProvider.when("/portal/module-gallery/list/:id", {
        controller: "ModuleGalleryController",
        templateUrl: "/app/app-portal/pages/module-gallery/list.html"
    });

    $routeProvider.when("/portal/module-data/details/:moduleId/:id", {
        controller: "SharedModuleDataController",
        templateUrl: "/app/app-portal/pages/moduleData/details.html"
    });

    $routeProvider.when("/portal/module-data/details/:moduleId", {
        controller: "SharedModuleDataController",
        templateUrl: "/app/app-portal/pages/moduleData/details.html"
    });

    $routeProvider.when("/portal/module/details/:id", {
        controller: "ModuleController",
        templateUrl: "/app/app-portal/pages/module/details.html"
    });
    
    $routeProvider.when("/portal/module-data/list/:moduleId", {
        controller: "ModuleDataController",
        templateUrl: "/app/app-portal/pages/module-data/list.html"
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


    $routeProvider.when("/portal/localize/list", {
        controller: "LanguageController",
        templateUrl: "/app/app-portal/pages/language/list.html"
    });

    $routeProvider.when("/portal/localize/details/:id", {
        controller: "LanguageController",
        templateUrl: "/app/app-portal/pages/language/details.html"
    });

    $routeProvider.when("/portal/localize/create", {
        controller: "LanguageController",
        templateUrl: "/app/app-portal/pages/language/details.html"
    });
    /* attribute set */
    $routeProvider.when("/portal/attribute-set/list", {
        controller: "AttributeSetController",
        templateUrl: "/app/app-portal/pages/attribute-set/list.html"
    });

    $routeProvider.when("/portal/attribute-set/details/:id", {
        controller: "AttributeSetController",
        templateUrl: "/app/app-portal/pages/attribute-set/details.html"
    });

    $routeProvider.when("/portal/attribute-set/create", {
        controller: "AttributeSetController",
        templateUrl: "/app/app-portal/pages/attribute-set/details.html"
    });

    $routeProvider.when("/portal/attribute-set-data/list", {
        controller: "AttributeSetDataController",
        templateUrl: "/app/app-portal/pages/attribute-set-data/list.html"
    });
    
    $routeProvider.when("/portal/attribute-set-data/create", {
        controller: "AttributeSetDataController",
        templateUrl: "/app/app-portal/pages/attribute-set-data/details.html"
    });
    
    $routeProvider.when("/portal/attribute-set-data/details", {
        controller: "AttributeSetDataController",
        templateUrl: "/app/app-portal/pages/attribute-set-data/details.html"
    });
   
    /* end attribute set*/

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
    
    $routeProvider.when("/portal/theme/export/:id", {
        controller: "ThemeController",
        templateUrl: "/app/app-portal/pages/theme/export.html"
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
    
    $routeProvider.when("/portal/my-profile", {
        controller: "UserController",
        templateUrl: "/app/app-portal/pages/user/my-profile.html"
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

    $routeProvider.when("/portal/language/list", {
        controller: "CultureController",
        templateUrl: "/app/app-portal/pages/culture/list.html"
    });
    $routeProvider.when("/portal/language/details/:id", {
        controller: "CultureController",
        templateUrl: "/app/app-portal/pages/culture/details.html"
    });
    $routeProvider.when("/portal/language/create", {
        controller: "CultureController",
        templateUrl: "/app/app-portal/pages/culture/details.html"
    });
    
    $routeProvider.when("/portal/messenger", {
        controller: "MessengerController",
        templateUrl: "/app/app-portal/pages/messenger/index.html"
    });

    $routeProvider.when("/portal/url-alias/list", {
        controller: "UrlAliasController",
        templateUrl: "/app/app-portal/pages/url-alias/list.html"
    });
    
    $routeProvider.when("/portal/url-alias/details/:id", {
        controller: "UrlAliasController",
        templateUrl: "/app/app-portal/pages/url-alias/details.html"
    });
   
    $routeProvider.when("/portal/social-feed", {
        controller: "SocialFeedController",
        templateUrl: "/app/app-portal/pages/social-feed/social-feed.html"
    });
    $routeProvider.otherwise({ redirectTo: "/portal" });
});
