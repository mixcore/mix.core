'use strict';
app.controller('ArticleController', ['$scope', '$rootScope', '$location', '$filter',
    'ngAppSettings', '$routeParams', 'ArticleService', 'UrlAliasService',
    function ($scope, $rootScope, $location, $filter, ngAppSettings, $routeParams, service, urlAliasService) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
        $scope.preview = function (item) {
            item.editUrl = '/portal/article/details/' + item.id;
            $rootScope.preview('article', item, item.title, 'modal-lg');
        };

        $scope.saveCallback = function () {
            $location.url($scope.referrerUrl);
        }
        $scope.saveFailCallback = function(){
            angular.forEach($scope.activedData.attributeSetNavs, function(nav){
                if(nav.isActived){
                    $scope.decryptAttributeSet(nav.attributeSet);
                }
            });
        }
        $scope.getSingleSuccessCallback = function () {
            var moduleId = $routeParams.module_id;
            var pageId = $routeParams.page_id;
            if (moduleId) {
                var moduleNav = $rootScope.findObjectByKey($scope.activedData.modules, 'moduleId', moduleId);
                if (moduleNav) {
                    moduleNav.isActived = true;
                }
            }
            if (pageId) {
                var pageNav = $rootScope.findObjectByKey($scope.activedData.categories, 'pageId', pageId);
                if (pageNav) {
                    pageNav.isActived = true;
                }
            }
            $scope.activedData.publishedDateTime = $filter('utcToLocalTime')($scope.activedData.publishedDateTime);
        }
        $scope.generateSeo = function () {
            if ($scope.activedData) {
                if ($scope.activedData.seoName === null || $scope.activedData.seoName === '') {
                    $scope.activedData.seoName = $rootScope.generateKeyword($scope.activedData.title, '-');
                }
                if ($scope.activedData.seoTitle === null || $scope.activedData.seoTitle === '') {
                    $scope.activedData.seoTitle = $scope.activedData.title
                }
                if ($scope.activedData.seoDescription === null || $scope.activedData.seoDescription === '') {
                    $scope.activedData.seoDescription = $scope.activedData.excerpt
                }
                if ($scope.activedData.seoKeywords === null || $scope.activedData.seoKeywords === '') {
                    $scope.activedData.seoKeywords = $scope.activedData.title
                }
            }
        }
        $scope.addAlias = async function () {
            var getAlias = await urlAliasService.getSingle();
            if (getAlias.isSucceed) {
                $scope.activedData.urlAliases.push(getAlias.data);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(getAlias.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        }

        $scope.removeAliasCallback = async function (index) {
            $scope.activedData.urlAliases.splice(index, 1);
            $scope.$apply();
        }
        $scope.validate = function(){
            angular.forEach($scope.activedData.attributeSetNavs, function(nav){
                if(nav.isActived){
                    $scope.encryptAttributeSet(nav.attributeSet);
                }
            });
            return true;
        }
        $scope.encryptAttributeSet = function(attributeSet){
            angular.forEach(attributeSet.attributes, function(attr){
                if(attr.isEncrypt){
                    angular.forEach(attributeSet.articleData.items, function(item){
                        var fieldData = $rootScope.findObjectByKey(item.data, 'attributeName', attr.name);
                        var encryptedData = $rootScope.encrypt(fieldData.stringValue);
                        fieldData.stringValue = encryptedData.data;
                        fieldData.encryptValue = encryptedData.data;
                        fieldData.encryptKey = encryptedData.key;
                        console.log(fieldData);
                    });
                }
            });
        }
        $scope.decryptAttributeSet = function(attributeSet){
            angular.forEach(attributeSet.attributes, function(attr){
                if(attr.isEncrypt){
                    angular.forEach(attributeSet.articleData.items, function(item){
                        var fieldData = $rootScope.findObjectByKey(item.data, 'attributeName', attr.name);
                        var encryptedData = {
                            key: fieldData.encryptKey,
                            data: fieldData.encryptValue
                        };
                        var decrypted = $rootScope.decrypt(encryptedData);
                        fieldData.stringValue = decrypted;
                    });
                }
            });
        }
    }
]);