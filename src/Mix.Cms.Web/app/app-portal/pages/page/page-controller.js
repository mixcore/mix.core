'use strict';
app.controller('PageController', ['$scope', '$rootScope', 'ngAppSettings', '$location', '$routeParams',
            'PageService','PageArticleService','PagePageService',
    function ($scope, $rootScope, ngAppSettings, $location, $routeParams, 
            service, pageArticleService, pagePageService) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);        
        $scope.request.query = 'level=0';       
        $scope.pageData={
            articles:[],
            products:[],
            data:[],
        };
        $scope.articleRequest = angular.copy(ngAppSettings.request);
        $scope.canDrag = $scope.request.orderBy !== 'Priority' || $scope.request.direction !== '0';
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
        $scope.getListSuccessCallback = function(){
            $scope.canDrag = $scope.request.orderBy !== 'Priority' || $scope.request.direction !== '0';
        };
        $scope.showChilds = function(id){
            $('#childs-'+ id).toggleClass('collapse');
        };
        $scope.updateInfos = async function (index) {
            $scope.data.items.splice(index, 1);
            $rootScope.isBusy = true;
            var startIndex = $scope.data.items[0].priority-1;
            for (var i = 0; i < $scope.data.items.length; i++) {
                $scope.data.items[i].priority = startIndex + i + 1;
            }
            var resp = await service.updateInfos($scope.data.items);
            if (resp && resp.isSucceed) {
                $scope.activedPage = resp.data;
                $rootScope.showMessage('success', 'success');
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        }
        $scope.goUp = async function (items, index) {
            items[index].priority -= 1; 
            items[index-1].priority += 1;             
        }
        
        $scope.goDown = async function (items, index) {
            items[index].priority += 1; 
            items[index-1].priority -= 1;             
        }
        
        $scope.updatePagePage = async function (items) {
            $rootScope.isBusy = true;            
            var resp = await pagePageService.updateInfos(items);
            if (resp && resp.isSucceed) {
                $scope.activedPage = resp.data;
                $rootScope.showMessage('success', 'success');
                $scope.getList();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }          
        }
        $scope.saveCallback = function () {
            $location.url($scope.referrerUrl);
        }
    }]);
