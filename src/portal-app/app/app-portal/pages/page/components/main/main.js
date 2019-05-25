
app.component('pageMain', {
    templateUrl: '/app/app-portal/pages/page/components/main/main.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', function ($rootScope, $scope) {
        var ctrl = this;
        ctrl.settings = $rootScope.globalSettings;
        ctrl.setPageType = function (type) {
            ctrl.page.type = $index;
        }
        ctrl.generateSeo = function () {
            if (ctrl.page) {
                if (ctrl.page.seoName === null || ctrl.page.seoName === '') {
                    ctrl.page.seoName = $rootScope.generateKeyword(ctrl.page.title, '-');
                }
                if (ctrl.page.seoTitle === null || ctrl.page.seoTitle === '') {
                    ctrl.page.seoTitle = $rootScope.generateKeyword(ctrl.page.title, '-');
                }
                if (ctrl.page.seoDescription === null || ctrl.page.seoDescription === '') {
                    ctrl.page.seoDescription = $rootScope.generateKeyword(ctrl.page.title, '-');
                }
                if (ctrl.page.seoKeywords === null || ctrl.page.seoKeywords === '') {
                    ctrl.page.seoKeywords = $rootScope.generateKeyword(ctrl.page.title, '-');
                }
            }
        }
    }],
    bindings: {
        page: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});