modules.component('contentFilter', {
    templateUrl: '/app/app-portal/components/content-filter/view.html',
    bindings: {
        query: '=',
        selected: '=',
        save: '&'
    },
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'PostService', 'PageService',
        function ($rootScope, $scope, ngAppSettings, postService, pageService) {
            var ctrl = this;
            ctrl.request = angular.copy(ngAppSettings.request);
            ctrl.types = ['Page', 'Post'];
            ctrl.type = 'Page';
            ctrl.navs = [];
            ctrl.data = { items: [] }
            ctrl.loadData = async function (pageIndex) {
                ctrl.request.query = ctrl.query + ctrl.srcId;
                ctrl.navs = [];
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

                switch (ctrl.type) {
                    case 'Page':
                        var response = await pageService.getList(ctrl.request);
                        if (response.isSucceed) {
                            ctrl.data = response.data;
                            $rootScope.isBusy = false;
                            $scope.$apply();
                        }
                        else {
                            $rootScope.showErrors(response.errors);
                            $rootScope.isBusy = false;
                            $scope.$apply();
                        }
                        break;
                    case 'Post':
                        var response = await postService.getList(ctrl.request);
                        if (response.isSucceed) {
                            ctrl.data = response.data;
                        }
                        else {
                            $rootScope.showErrors(response.errors);
                            $rootScope.isBusy = false;
                            $scope.$apply();
                        }
                        break;
                }                
            };
            ctrl.saveSelected = function () {
                ctrl.selected = $rootScope.filterArray(ctrl.data, 'isActived', true);
                setTimeout(() => {
                    ctrl.save().then(() => {
                        ctrl.loadPosts();
                    });

                }, 500);

            };
            ctrl.limString = function(str, max){          
                if(str){  
                    return (str.length>max)?  str.substring(0, max) + ' ...': str;
                }
            };
        }

    ]
});