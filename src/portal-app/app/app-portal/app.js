'use strict';
var app = angular.module('MixPortal', ['ngRoute', 'components', 'ngFileUpload', 'LocalStorageModule',
    'bw.paging', 'dndLists', 'ngTagsInput', 'ngSanitize']);
var modules = angular.module('components', []);
var $routeProviderReference;
var currentRoute;
app.run(['$route', '$http', '$rootScope', 'AppSettings', 'ngAppSettings', 
function ($route, $http, $rootScope, appSettings, ngAppSettings) {  
    $http({method: 'GET', url: appSettings.serviceBase + '/api/' + appSettings.apiVersion + '/portal/jarray-data/portal-menus.json'}).then(function (resp) {
        ngAppSettings.routes = resp.data.data;
        
        angular.forEach(ngAppSettings.routes, function(item, key) {
            if(item.type=='item'){
                $routeProviderReference.when(item.path, {
                    controller: item.controller,
                    templateUrl: item.templatePath
                });
                angular.forEach(item.subMenus.data, function(sub, key) {
                    $routeProviderReference.when(sub.path, {
                        controller: sub.controller,
                        templateUrl: sub.templatePath
                    });
                });
            }
            else{
                angular.forEach(item.data, function(sub, key) {
                    $routeProviderReference.when(sub.path, {
                        controller: sub.controller,
                        templateUrl: sub.templatePath
                    });
                    angular.forEach(sub.subMenus.data, function(sub1, key) {
                        $routeProviderReference.when(sub1.path, {
                            controller: sub1.controller,
                            templateUrl: sub1.templatePath
                        });
                    });
                });
            }
        });
        $route.reload();			
    });  
}]);  