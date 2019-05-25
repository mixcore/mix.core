
modules.component('moduleDataPreview', {
    templateUrl: '/app/app-shared/components/module-data-preview/view.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
    }
    ],
    bindings: {
        data: '=',
        width: '=',
    }
});