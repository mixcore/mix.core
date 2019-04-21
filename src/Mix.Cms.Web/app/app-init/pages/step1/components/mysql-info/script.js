modules.component('mysqlInfo', {
    templateUrl: '/app/app-init/pages/step1/components/mysql-info/view.html',
    controller: ['$rootScope',
        function ($rootScope) {
            var ctrl = this;
        }
    ],
    bindings: {
        initCmsModel: '='
    }
});