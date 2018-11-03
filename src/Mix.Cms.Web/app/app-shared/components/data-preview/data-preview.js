modules.component('swDataPreview', {
    templateUrl: '/app/app-shared/components/data-preview/data-preview.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
    }
    ],
    bindings: {
        type: '=',
        value: '=',
    }
});