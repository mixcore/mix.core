'use strict';
app.controller('ModalPostController', [
    '$scope', '$rootScope', '$location', 'ngAppSettings', '$routeParams', 'PostService',
    function (
        $scope, $rootScope, $location, ngAppSettings, $routeParams, service) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
        $scope.columns = [{
                title: 'Title',
                name: 'title',
                filter: true,
                type: 0 // string - ngAppSettings.dataTypes[7]
            },           
            {
                title: 'Url',
                name: 'imageUrl',
                filter: true,
                type: 16 // string - ngAppSettings.dataTypes[7]
            },
        ];
    }
]);