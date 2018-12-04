'use strict';
app.controller('PageController', 
            ['$scope', '$rootScope', '$routeParams', 'ngAppSettings', 'PageService','PageArticleService',
    function ($scope, $rootScope, $routeParams, ngAppSettings, service, pageArticleService) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);        
        $scope.request.query = 'level=0';       
        $scope.pageData={
            articles:[],
            products:[],
            data:[],
        };
        $scope.articleRequest = angular.copy(ngAppSettings.request);
        $scope.loadArticles = async function () {
            $rootScope.isBusy = true;
            var id = $routeParams.id;
            $scope.articleRequest.query += '&page_id='+id;
            var response = await pageArticleService.getList($scope.articleRequest);
            if (response.isSucceed) {
                $scope.pageData.articles = response.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.updateInfos = async function (items) {
            $rootScope.isBusy = true;
            var resp = await service.updateInfos(items);
            if (resp && resp.isSucceed) {
                $scope.activedData = resp.data;
                $rootScope.showMessage('success', 'success');
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

    }]);
