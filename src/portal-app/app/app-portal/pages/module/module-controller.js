'use strict';
app.controller('ModuleController', ['$scope', '$rootScope', 'ngAppSettings', '$location', '$routeParams',
    'ModuleService', 'SharedModuleDataService', 'RelatedAttributeSetDataService',
    function ($scope, $rootScope, ngAppSettings, $location, $routeParams, moduleServices, moduleDataService, relatedAttributeSetDataService) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, moduleServices, 'product');
        $scope.contentUrl = '';
        $scope.getSingleSuccessCallback = function () {
            if ($scope.activedData.id > 0) {
                // module => list post or list product
                if ($scope.activedData.type == 2 || $scope.activedData.type == 6) {
                    $scope.contentUrl = '/portal/module-post/list/' + $scope.activedData.id;
                }
                else {
                    $scope.contentUrl = '/portal/module/data/' + $scope.activedData.id;
                }
            }
        };
        $scope.getListByType = async function (pageIndex) {
            $scope.request.query = '?type=' + $scope.type;
            await $scope.getList(pageIndex);
        };
        $scope.defaultAttr = {
            name: '',
            options: [],
            priority: 0,
            dataType: 7,
            isGroupBy: false,
            isSelect: false,
            isDisplay: true,
            width: 3
        };
        $scope.type = '-1';

        $scope.settings = $rootScope.globalSettings;
        $scope.activedData = null;
        $scope.editDataUrl = '';

        $scope.loadModuleDatas = async function () {
            $rootScope.isBusy = true;
            var id = $routeParams.id;
            $scope.dataColumns = [];
            var response = await moduleServices.getSingle([id, 'mvc']);
            if (response.isSucceed) {

                $scope.activedData = response.data;
                $scope.editDataUrl = '/portal/module-data/details/' + $scope.activedData.id;
                $scope.loadMoreModuleDatas();
                angular.forEach($scope.activedData.columns, function (e, i) {
                    if (e.isDisplay) {
                        $scope.dataColumns.push({
                            title: e.title,
                            name: e.name,
                            filter: true,
                            type: 0 // string - ngAppSettings.dataTypes[0]
                        });
                    }
                });
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };

        $scope.loadMoreModuleDatas = async function (pageIndex) {
            $scope.request.query = '&module_id=' + $scope.activedData.id;
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
            $rootScope.isBusy = true;
            var resp = await moduleDataService.getModuleDatas($scope.request);
            if (resp && resp.isSucceed) {

                $scope.activedData.data = resp.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        $scope.exportModuleData = async function (pageIndex) {
            $scope.request.query = '&module_id=' + $scope.activedData.id;
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
            $rootScope.isBusy = true;
            var resp = await moduleDataService.exportModuleData($scope.request);
            if (resp && resp.isSucceed) {
                window.top.location = resp.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                if (resp) { $rootScope.showErrors(resp.errors); }
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };


        $scope.removeData = function (id) {
            if ($scope.activedData) {
                $rootScope.showConfirm($scope, 'removeDataConfirmed', [id], null, 'Remove Data', 'Are you sure');
            }
        }

        $scope.removeDataConfirmed = async function (id) {
            $rootScope.isBusy = true;
            var result = await moduleDataService.removeModuleData(id);
            if (result.isSucceed) {
                $scope.loadModuleDatas();
            }
            else {
                $rootScope.showMessage('failed');
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        }
        $scope.updateModuleDataField = async function (item, propertyName) {
            var result = await moduleDataService.saveFields(item.id, propertyName, item[propertyName]);
            if (result.isSucceed) {
                $scope.loadModuleDatas();
            }
            else {
                $rootScope.showMessage('failed');
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        }
        $scope.updateDataInfos = async function (items) {
            $rootScope.isBusy = true;
            var resp = await moduleDataService.updateInfos(items);
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
        };
        // $scope.saveSuccessCallback = function () {
        //     $location.url($scope.referrerUrl);
        // }
        $scope.loadPosts = async function () {
            $rootScope.isBusy = true;
            var id = $routeParams.id;
            $scope.postRequest.query += '&page_id=' + id;
            var response = await pagePostService.getList($scope.postRequest);
            if (response.isSucceed) {
                $scope.pageData.posts = response.data;
                $rootScope.isBusy = false;
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        };
        $scope.selectedCol = null;
        $scope.dragoverCallback = function (index, item, external, type) {
            //console.log('drop ', index, item, external, type);
        }
        $scope.insertColCallback = function (index, item, external, type) {
        }
        $scope.addAttr = function () {
            if ($scope.activedData.attributeData.data.values) {
                var t = angular.copy($scope.defaultAttr);
                t.priority = $scope.activedData.attributeData.data.values.length + 1;
                ctrl.fields.push(t);
            }
        };
        $scope.removeAttribute = function (attr, index) {
            $rootScope.showConfirm($scope, 'removeAttributeConfirmed', [attr, index], null, 'Remove Field', 'Are you sure');
        };
        $scope.removeAttributeConfirmed = function (attr, index) {
            relatedAttributeSetDataService.delete([])
            $scope.activedData.attributeData.data.values.splice(index, 1);
        };
    }]);
