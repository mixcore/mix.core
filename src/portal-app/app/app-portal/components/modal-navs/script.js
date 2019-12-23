modules.component('modalNavs', {
    templateUrl: '/app/app-portal/components/modal-navs/view.html',
    bindings: {
        modelName:'=',
        viewType:'=',        
        selects:'=',        
        isSingle:'=?',
        isGlobal:'=?',
        save: '&'
    },
    controller: ['$rootScope', '$scope', '$routeParams', 'ngAppSettings',
        function ($rootScope, $scope, $routeParams, ngAppSettings) {            
            var ctrl = this;
            ctrl.request = angular.copy(ngAppSettings.request);
            ctrl.contentStatuses = angular.copy(ngAppSettings.contentStatuses);
            ctrl.activedData = null;
            ctrl.data = null;
            ctrl.isInit = false;
            ctrl.isValid = true;            
            ctrl.errors = [];
            ctrl.selected = [];
            
            ctrl.init = function(){                
                ctrl.service = $rootScope.getODataService(ctrl.modelName, ctrl.isGlobal);
                ctrl.prefix = 'modal_navs_' + ctrl.modelName;
                ctrl.cols = ctrl.selects.split(',');
                ctrl.getList();
            };
            ctrl.count = async function () {
                $rootScope.isBusy = true;
                var resp = await ctrl.service.count(ctrl.viewType);
                if (resp) {
                    ctrl.request.totalItems = resp;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                } else {
                    if (resp) {
                        $rootScope.showErrors('Failed');
                    }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            ctrl.getList = async function (pageIndex) {
                if (pageIndex !== undefined) {
                    ctrl.request.pageIndex = pageIndex;
                }
                if (ctrl.request.fromDate !== null) {
                    var d = new Date(ctrl.request.fromDate);
                    ctrl.request.fromDate = d.toISOString();
                }
                if (ctrl.request.toDate !== null) {
                    var d = new Date(ctrl.request.toDate);
                    ctrl.request.toDate = d.toISOString();
                }
                var resp = await ctrl.service.getList(ctrl.viewType, ctrl.request);
                if (resp) {            
                    ctrl.data = resp;
                    ctrl.count();
                    $.each(ctrl.data, function (i, data) {
                        $.each(ctrl.activedDatas, function (i, e) {
                            if (e.dataId === data.id) {
                                data.isHidden = true;
                            }
                        });
                    });
                    $rootScope.isBusy = false;
                    $scope.$apply();
                } else {
                    if (resp) {
                        $rootScope.showErrors('Failed');
                    }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };

            // ctrl.loadData = async function (pageIndex) {   
            //     ctrl.request.query = ctrl.query + ctrl.srcId;    
            //     if (pageIndex !== undefined) {
            //         ctrl.request.pageIndex = pageIndex;
            //     }
            //     if (ctrl.request.fromDate !== null) {
            //         var d = new Date(ctrl.request.fromDate);
            //         ctrl.request.fromDate = d.toISOString();
            //     }
            //     if (ctrl.request.toDate !== null) {
            //         var d = new Date(ctrl.request.toDate);
            //         ctrl.request.toDate = d.toISOString();
            //     }
            //     var response = await pageService.getList(ctrl.request);
            //     if (response.isSucceed) {
            //         ctrl.data = response.data;
            //         ctrl.navs = [];
            //         angular.forEach(response.data.items, function (e) {
            //             var item = {
            //                 priority: e.priority,
            //                 description: e.title,
            //                 pageId: e.id,                            
            //                 image: e.thumbnailUrl,
            //                 specificulture: e.specificulture,
            //                 status: 2,
            //                 isActived: false
            //             };
            //             item[ctrl.srcField] = ctrl.srcId;
            //             ctrl.navs.push(item);
            //         });
            //         $rootScope.isBusy = false;
            //         $scope.$apply();
            //     }
            //     else {
            //         $rootScope.showErrors(response.errors);
            //         $rootScope.isBusy = false;
            //         $scope.$apply();
            //     }
            // }
            ctrl.selectAll = function(isSelectAll){
                angular.forEach(ctrl.data, element => {
                    element.isActived = isSelectAll;
                });
            };
            ctrl.selectChange = function(item){
                if(ctrl.isSingle == 'true' && item.isActived){
                    angular.forEach(ctrl.data, element => {
                        element.isActived = false;
                    }); 
                    item.isActived = true;
                }
            };
            ctrl.saveSelected = function(){
                ctrl.selected = $rootScope.filterArray(ctrl.data, ['isActived'], [true]);
                if(ctrl.save){
                    ctrl.save({selected: ctrl.selected});
                }
            };
        }

    ],
    
});