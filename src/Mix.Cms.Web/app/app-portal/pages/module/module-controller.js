'use strict';
app.controller('ModuleController', ['$scope', '$rootScope', 'ngAppSettings', '$routeParams',
    'ModuleService', 'SharedModuleDataService',
    function ($scope, $rootScope, ngAppSettings, $routeParams, moduleServices, moduleDataService) {
        BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, moduleServices, 'product');
        $scope.contentUrl = '';
        $scope.getSingleSuccessCallback = function () {
            if ($scope.activedData.id > 0) {
                $scope.contentUrl = '/portal/module/data/' + $scope.activedData.id;
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
        $scope.type='-1';
        
        $scope.settings = $rootScope.globalSettings;
        //$scope.dataTypes = ngAppSettings.dataTypes;
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
        $scope.addAttr = function () {
            if ($scope.activedData) {
                var t = angular.copy($scope.defaultAttr);
                $scope.activedData.columns.push(t);
            }
        };

        $scope.addOption = function (col, index) {
            var val = angular.element('#option_' + index).val();
            col.options.push(val);
            angular.element('#option_' + index).val('');
        };
        $scope.generateForm = function(){
            var formHtml = document.createElement('module-form');
            formHtml.setAttribute('class','row');
            formHtml.setAttribute('ng-controller','ModuleFormController');
            formHtml.setAttribute("ng-init", "initModuleForm('"+ $scope.activedData.name + "')");
            angular.forEach($scope.activedData.columns, function(e,i){
                console.log(e);
                var el;
                var label = document.createElement('label');
                label.setAttribute('class', 'control-label');
                label.setAttribute('ng-bind', '{{data.title}}');
                
                switch(e.dataType){
                    case 1:
                    el = document.createElement('input');
                    el.setAttribute('type', 'datetime-local');                                 
                    break;
                    
                    case 2:
                    el = document.createElement('input');
                    el.setAttribute('type', 'date');                                 
                    break;
                    
                    case 3:
                    el = document.createElement('input');
                    el.setAttribute('type', 'time');                                 
                    break;

                    case 5:
                    el = document.createElement('input');
                    el.setAttribute('type', 'tel');                                 
                    break;
                   
                    case 6:
                    el = document.createElement('input');
                    el.setAttribute('type', 'number');                                 
                    break;
                   
                    case 8:
                    el = document.createElement('trumbowyg');
                    el.setAttribute('options', '{}');                                 
                    el.setAttribute('type', 'number');                                 
                    break;
                    
                    case 9:
                    el = document.createElement('textarea');
                    break;

                    default:
                    el = document.createElement('input');
                    el.setAttribute('type', 'text');
                    formHtml.appendChild(el);
                    break;
                }
                el.setAttribute('ng-model', 'data.jItem[' + e.name + '].value');
                el.setAttribute( 'placeholder', '{{$ctrl.title}}');
                formHtml.appendChild(label);      
                formHtml.appendChild(el);      
                
            });
            console.log(formHtml);
            $scope.activedData.formView.content = formHtml.innerHTML;
        };

        $scope.generateName = function (col) {
            col.name = $rootScope.generateKeyword(col.title, '_');
        }
        $scope.removeAttr = function (index) {
            if ($scope.activedData) {

                $scope.activedData.columns.splice(index, 1);
            }
        }

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
        $scope.selectedCol = null;
        $scope.dragoverCallback = function (index, item, external, type) {
            //console.log('drop ', index, item, external, type);
        }
        $scope.insertColCallback = function (index, item, external, type) {
            console.log('insert ', index, item, external, type);
        }
    }]);
