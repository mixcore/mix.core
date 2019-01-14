modules.component('modalNavArticles', {
    templateUrl: '/app/app-portal/components/modal-nav-articles/modal-nav-articles.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'ArticleService',
        function ($rootScope, $scope, ngAppSettings, articleService) {
            var ctrl = this;
            ctrl.request = angular.copy(ngAppSettings.request);            
            ctrl.navs = [];
            ctrl.data = { items: [] }
            ctrl.loadArticles = async function (pageIndex) {   
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
                var response = await articleService.getList(ctrl.request);
                if (response.isSucceed) {
                    ctrl.data = response.data;
                    ctrl.navs = [];
                    angular.forEach(response.data.items, function (e) {
                        var item = {
                            priority: e.priority,
                            description: e.title,
                            articleId: e.id,                            
                            image: e.thumbnailUrl,
                            specificulture: $rootScope.globalSettingsService.get('lang'),
                            article: e,
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
                        ctrl.loadArticles();
                    });   
                     
                }, 500);
                
            }
        }

    ],
    bindings: {
        srcField: '=',
        srcId: '=',
        query:'=',
        selected:'=',
        save: '&'
    }
});