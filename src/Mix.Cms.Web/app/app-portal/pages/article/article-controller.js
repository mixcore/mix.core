'use strict';
app.controller('ArticleController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location', 'ArticleServices',
    function ($scope, $rootScope, ngAppSettings, $routeParams, $location, articleServices) {

        $scope.request = angular.copy(ngAppSettings.request);

        $scope.activedArticle = null;
        $scope.relatedArticles = [];
        $rootScope.isBusy = false;
        $scope.data = {
            pageIndex: 0,
            pageSize: 1,
            totalItems: 0
        };
        $scope.errors = [];

        $scope.range = function (max) {
            var input = [];
            for (var i = 1; i <= max; i += 1) input.push(i);
            return input;
        };

        $scope.loadArticle = async function () {

            $rootScope.isBusy = true;
            var id = $routeParams.id;
            var response = await articleServices.getArticle(id, 'portal');
            if (response.isSucceed) {
                $scope.activedArticle = response.data;
                $rootScope.initEditor();
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        $scope.loadArticles = async function (pageIndex) {
            if (pageIndex !== undefined) {
                $scope.request.pageIndex = pageIndex;
            }
            if ($scope.request.fromDate !== null) {
                var d = new Date($scope.request.fromDate);
                $scope.request.fromDate = d.toISOString();
            }
            if ($scope.request.toDate !== null) {
                var d = new Date($scope.request.toDate);
                $scope.request.toDate = d.toISOString();                
            }
            var resp = await articleServices.getArticles($scope.request);
            if (resp && resp.isSucceed) {

                ($scope.data = resp.data);
                //$("html, body").animate({ "scrollTop": "0px" }, 500);
                $.each($scope.data.items, function (i, article) {

                    $.each($scope.activedArticles, function (i, e) {
                        if (e.articleId === article.id) {
                            article.isHidden = true;
                        }
                    })
                })
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.removeArticle = function (id) {
            $rootScope.showConfirm($scope, 'removeArticleConfirmed', [id], null, 'Remove Article', 'Are you sure');
        }

        $scope.removeArticleConfirmed = async function (id) {
            $rootScope.isBusy = true;
            var result = await articleServices.removeArticle(id);
            if (result.isSucceed) {
                $scope.loadArticles();
            }
            else {
                $rootScope.showErrors(result.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        }


        $scope.saveArticle = async function (article) {
            article.content = $('.editor-content').val();
            $rootScope.isBusy = true;
            var resp = await articleServices.saveArticle(article);
            if (resp && resp.isSucceed) {
                $scope.activedArticle = resp.data;
                $rootScope.showMessage('Thành công', 'success');
                $rootScope.isBusy = false;
                $location.path('/portal/article/list');
                $scope.$apply();
                //$location.path('/portal/article/details/' + resp.data.id);
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

    }]);
