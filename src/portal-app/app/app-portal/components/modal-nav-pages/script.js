modules.component('modalNavPages', {
    templateUrl: '/app/app-portal/components/modal-nav-pages/view.html',
    bindings: {
        srcField: '=',
        srcId: '=',
        query:'=',
        selected:'=',
        save: '&'
    },
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'PageService',
        function ($rootScope, $scope, ngAppSettings, pageService) {
            var ctrl = this;
            ctrl.request = angular.copy(ngAppSettings.request);            
            ctrl.navs = [];
            ctrl.data = { items: [] }
            ctrl.loadData = async function (pageIndex) {   
                ctrl.request.query = ctrl.query + ctrl.srcId;    
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
                var response = await pageService.getList(ctrl.request);
                if (response.isSucceed) {
                    ctrl.data = response.data;
                    ctrl.navs = [];
                    angular.forEach(response.data.items, function (e) {
                        var item = {
                            priority: e.priority,
                            description: e.title,
                            pageId: e.id,                            
                            image: e.thumbnailUrl,
                            specificulture: e.specificulture,
                            status: 2,
                            isActived: false
                        };
                        item[ctrl.srcField] = ctrl.srcId;
                        ctrl.navs.push(item);
                    });
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    $rootScope.showErrors(response.errors);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            }
            ctrl.saveSelected = function(){
                ctrl.selected = $rootScope.filterArray(ctrl.navs, 'isActived', true);
                setTimeout(() => {
                    ctrl.save().then(() => {
                        ctrl.loadPages();
                    });   
                     
                }, 500);
                
            }
        }

    ],
    
});