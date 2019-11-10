'use strict';
app.controller('MixAttributeSetDataController',
    ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$location', 'MixAttributeSetDataService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $location, service) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            $scope.queries = {};
            $scope.settings = $rootScope.globalSettings;
            $scope.canDrag = $scope.request.orderBy !== 'Priority' || $scope.request.direction !== '0';
            $scope.init = async function () {
                $scope.attributeSetId = $routeParams.attributeSetId;
                $scope.attributeSetName = $routeParams.attributeSetName;
                $scope.dataId = $routeParams.dataId;
                $scope.request.query = 'attributeSetId=' + $routeParams.attributeSetId;
                $scope.request.query += '&attributeSetName=' + $routeParams.attributeSetName;
            };

            $scope.preview = function (item) {
                item.editUrl = '/portal/post/details/' + item.id;
                $rootScope.preview('post', item, item.title, 'modal-lg');
            };
            $scope.edit = function (data) {
                $scope.goToPath('/portal/attribute-set-data/details?dataId=' + data.id + '&attributeSetId=' + $scope.attributeSetId)
            };
            $scope.remove = function (data) {
                $rootScope.showConfirm($scope, 'removeConfirmed', [data.id], null, 'Remove', 'Are you sure');
            };

            $scope.removeConfirmed = async function (dataId) {
                $rootScope.isBusy = true;
                var result = await service.delete([dataId]);
                if (result.isSucceed) {
                    if ($scope.removeCallback) {
                        $rootScope.executeFunctionByName('removeCallback', $scope.removeCallbackArgs, $scope)
                    }
                    $scope.getList();
                }
                else {
                    $rootScope.showMessage('failed');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            $scope.sendMail = function (data) {
                var email = '';
                angular.forEach(data.values, function(e){
                    if(e.attributeFieldName == 'email'){
                        email = e.stringValue;
                    }
                })
                $rootScope.showConfirm($scope, 'sendMailConfirmed', [data], null, 'Send mail', 'Are you sure to send mail to '+ email);
            };
            $scope.sendMailConfirmed = async function(data){
                $rootScope.isBusy = true;
                $rootScope.isBusy = true;
                var result = await service.sendMail([data.id]);
                if (result.isSucceed) {
                    $rootScope.showMessage('success', 'success');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    $rootScope.showMessage('failed');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            $scope.saveOthers = async function () {
                var response = await service.saveList($scope.others);
                if (response.isSucceed) {
                    $scope.getList();
                    $scope.$apply();
                }
                else {
                    $rootScope.showErrors(response.errors);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            $scope.getList = async function (pageIndex) {
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
                $scope.request.query = '';
                $scope.request.query = 'attributeSetId=' + $routeParams.attributeSetId;
                $scope.request.query += '&attributeSetName=' + $routeParams.attributeSetName;
                Object.keys($scope.queries).forEach(e => {
                    if($scope.queries[e]){
                        $scope.request.query += '&' + e + '=' + $scope.queries[e];
                    }
                });
                $rootScope.isBusy = true;
                var resp = await service.getList($scope.request);
                if (resp && resp.isSucceed) {

                    $scope.data = resp.data;
                    $.each($scope.data.items, function (i, data) {

                        $.each($scope.activedDatas, function (i, e) {
                            if (e.dataId === data.id) {
                                data.isHidden = true;
                            }
                        });
                    });
                    if ($scope.getListSuccessCallback) {
                        $scope.getListSuccessCallback();
                    }
                    $("html, body").animate({ "scrollTop": "0px" }, 500);
                    if (!resp.data || !resp.data.items.length) {
                        $scope.queries = {};
                    }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                } else {
                    if (resp) {
                        $rootScope.showErrors(resp.errors);
                    }
                    if ($scope.getListFailCallback) {
                        $scope.getListFailCallback();
                    }
                    $scope.queries = {};
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            $scope.export = async function (pageIndex) {
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
                $scope.request.query = '';
                $scope.request.query = 'attributeSetId=' + $routeParams.attributeSetId;
                $scope.request.query += '&attributeSetName=' + $routeParams.attributeSetName;
                Object.keys($scope.queries).forEach(e => {
                    if($scope.queries[e]){
                        $scope.request.query += '&' + e + '=' + $scope.queries[e];
                    }
                });
                $rootScope.isBusy = true;
                var resp = await service.export($scope.request);
                if (resp && resp.isSucceed) {

                    window.top.location = resp.data;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                } else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
        }]);
